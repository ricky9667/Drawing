using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Drive.v2;
using Google.Apis.Auth.OAuth2;
using System.IO;
using System.Threading;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Download;
using Google.Apis.Drive.v2.Data;
using System.Net;

namespace DrawingModel
{
    public class GoogleDriveService
    {
        private static readonly string[] _scopes = new[] { DriveService.Scope.DriveFile, DriveService.Scope.Drive };
        private DriveService _service;
        private const int KB = 0x400;
        private const int DOWNLOAD_CHUNK_SIZE = 256 * KB;
        private int _timeStamp;
        private string _applicationName;
        private string _clientSecretFileName;
        private UserCredential _credential;

        // init service
        public GoogleDriveService(string applicationName, string clientSecretFileName)
        {
            _applicationName = applicationName;
            _clientSecretFileName = clientSecretFileName;
            this.CreateNewService(applicationName, clientSecretFileName);
        }

        // create new service
        private void CreateNewService(string applicationName, string clientSecretFileName)
        {
            const string USER = "user";
            const string CREDENTIAL_FOLDER = ".credential/";

            using (FileStream stream = new FileStream(clientSecretFileName, FileMode.Open, FileAccess.Read))
            {
                string credentialPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                credentialPath = Path.Combine(credentialPath, CREDENTIAL_FOLDER + applicationName);
                _credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets, _scopes, USER, CancellationToken.None, new FileDataStore(credentialPath, true)).Result;
            }
            
            _timeStamp = UNIXNowTimeStamp;
            _service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _credential,
                ApplicationName = applicationName
            });
        }

        private int UNIXNowTimeStamp
        {
            get
            {
                const int UNIX_START_YEAR = 1970;
                DateTime unixStartTime = new DateTime(UNIX_START_YEAR, 1, 1);
                return Convert.ToInt32((DateTime.Now.Subtract(unixStartTime).TotalSeconds));
            }
        }

        //Check and refresh the credential if credential is out-of-date
        private void CheckCredentialTimeStamp()
        {
            const int ONE_HOUR_SECOND = 3600;
            int nowTimeStamp = UNIXNowTimeStamp;

            if ((nowTimeStamp - _timeStamp) > ONE_HOUR_SECOND)
                this.CreateNewService(_applicationName, _clientSecretFileName);
        }

        // list google drive files and folders
        public List<Google.Apis.Drive.v2.Data.File> ListRootFileAndFolder()
        {
            List<Google.Apis.Drive.v2.Data.File> returnList = new List<Google.Apis.Drive.v2.Data.File>();
            const string ROOT_QUERY_STRING = "'root' in parents";

            try
            {
                returnList = ListFileAndFolderWithQueryString(ROOT_QUERY_STRING);
            }
            catch (Exception exception)
            {
                throw exception;
            }

            return returnList;
        }

        // get root files
        private List<Google.Apis.Drive.v2.Data.File> ListFileAndFolderWithQueryString(string queryString)
        {
            List<Google.Apis.Drive.v2.Data.File> returnList = new List<Google.Apis.Drive.v2.Data.File>();
            this.CheckCredentialTimeStamp();
            FilesResource.ListRequest listRequest = _service.Files.List();
            listRequest.Q = queryString;
            do
            {
                returnList.AddRange(GetFileListItems(listRequest));
            } while (!String.IsNullOrEmpty(listRequest.PageToken));

            return returnList;
        }

        // get filelist items
        private List<Google.Apis.Drive.v2.Data.File> GetFileListItems(FilesResource.ListRequest listRequest)
        {
            List<Google.Apis.Drive.v2.Data.File> returnList = new List<Google.Apis.Drive.v2.Data.File>();
            try
            {
                FileList fileList = listRequest.Execute();
                returnList.AddRange(fileList.Items);
                listRequest.PageToken = fileList.NextPageToken;
            }
            catch (Exception exception)
            {
                listRequest.PageToken = null;
                throw exception;
            }
            return returnList;
        }

        // upload
        public Google.Apis.Drive.v2.Data.File UploadFile(string uploadFileName, string contentType, Action<IUploadProgress> uploadProgressEventHandler = null, Action<Google.Apis.Drive.v2.Data.File> responseReceivedEventHandler = null)
        {
            FileStream uploadStream = new FileStream(uploadFileName, FileMode.Open, FileAccess.Read);
            const char SPLASH = '\\';
            CheckCredentialTimeStamp();

            Google.Apis.Drive.v2.Data.File fileToInsert = new Google.Apis.Drive.v2.Data.File();
            fileToInsert.Title = (uploadFileName.LastIndexOf(SPLASH) != -1) ? uploadFileName.Substring(uploadFileName.LastIndexOf(SPLASH) + 1) : uploadFileName;
            FilesResource.InsertMediaUpload insertRequest = _service.Files.Insert(fileToInsert, uploadStream, contentType);
            insertRequest.ChunkSize = FilesResource.InsertMediaUpload.MinimumChunkSize + FilesResource.InsertMediaUpload.MinimumChunkSize;
            if (uploadProgressEventHandler != null)
                insertRequest.ProgressChanged += uploadProgressEventHandler;
            if (responseReceivedEventHandler != null)
                insertRequest.ResponseReceived += responseReceivedEventHandler;
            return MakeUploadRequest(insertRequest, uploadStream);
        }

        // run upload request
        private Google.Apis.Drive.v2.Data.File MakeUploadRequest(FilesResource.InsertMediaUpload insertRequest, FileStream uploadStream)
        {
            try
            {
                insertRequest.Upload();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                uploadStream.Close();
            }

            return insertRequest.ResponseBody;
        }

        // download
        public void DownloadFile(Google.Apis.Drive.v2.Data.File fileToDownload, string downloadPath, Action<IDownloadProgress> downloadProgressChangedEventHandler = null)
        {
            const string SPLASH = @"\";

            CheckCredentialTimeStamp();
            if (!String.IsNullOrEmpty(fileToDownload.DownloadUrl))
            {
                try
                {
                    Task<byte[]> downloadByte = _service.HttpClient.GetByteArrayAsync(fileToDownload.DownloadUrl);
                    byte[] byteArray = downloadByte.Result;
                    string downloadPosition = downloadPath + SPLASH + fileToDownload.Title;
                    System.IO.File.WriteAllBytes(downloadPosition, byteArray);
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
        }

        // delete
        public void DeleteFile(string fileId)
        {
            CheckCredentialTimeStamp();
            try
            {
                _service.Files.Delete(fileId).Execute();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        // update
        public Google.Apis.Drive.v2.Data.File UpdateFile(string fileName, string fileId, string contentType)
        {
            CheckCredentialTimeStamp();
            try
            {
                Google.Apis.Drive.v2.Data.File file = _service.Files.Get(fileId).Execute();
                byte[] byteArray = System.IO.File.ReadAllBytes(fileName);
                MemoryStream stream = new MemoryStream(byteArray);
                FilesResource.UpdateMediaUpload request = _service.Files.Update(file, fileId, stream, contentType);
                request.NewRevision = true;
                request.Upload();

                Google.Apis.Drive.v2.Data.File updatedFile = request.ResponseBody;
                return updatedFile;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
