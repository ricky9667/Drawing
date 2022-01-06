using System.Collections.Generic;
using System.IO;

namespace DrawingModel
{
    public class FileHandler
    {
        private readonly Model _model;
        private GoogleDriveService _service = null;
        private const char SPACE = ' ';
        private const string FILE_NAME = "Shapes.txt";
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), FILE_NAME);
        public FileHandler(Model model)
        {
            _model = model;
        }

        // init google drive service
        public void SetUpGoogleDriveService()
        {
            const string APPLICATION_NAME = "Drawing";
            const string CLIENT_SECRET_FILE_NAME = "clientSecret.json";
            string clientSecret = Path.Combine(Directory.GetCurrentDirectory(), CLIENT_SECRET_FILE_NAME);
            if (_service == null)
                _service = new GoogleDriveService(APPLICATION_NAME, clientSecret);
        }

        // save shapes to local text file
        public void SaveShapesToLocal()
        {
            using (StreamWriter outputFile = new StreamWriter(_filePath))
            {
                foreach (IShape shape in _model.Shapes)
                {
                    if (shape.ShapeType == ShapeType.LINE)
                    {
                        Line line = shape as Line;
                        outputFile.WriteLine(line.ShapeType.ToString() + SPACE + _model.Shapes.IndexOf(line.FirstShape) + SPACE + _model.Shapes.IndexOf(line.SecondShape));
                    }
                    else
                    {
                        outputFile.WriteLine(shape.ShapeType.ToString() + SPACE + shape.X1 + SPACE + shape.Y1 + SPACE + shape.X2 + SPACE + shape.Y2);
                    }
                }
            }
        }

        // load shapes from local text file
        public void LoadShapesFromLocal()
        {
            _model.Shapes.Clear();
            foreach (string row in File.ReadAllLines(_filePath))
            {
                string[] data = row.Split(SPACE);
                IShape shape = ShapeFactory.CreateShape(data[0]);
                if (shape.ShapeType == ShapeType.LINE)
                {
                    Line line = shape as Line;
                    line.SetReferenceShapes(_model.Shapes[int.Parse(data[1])], _model.Shapes[int.Parse(data[2])]);
                    _model.Shapes.Add(line);
                }
                else
                {
                    shape.SetShapeCoordinates(int.Parse(data[1]), int.Parse(data[2]), int.Parse(data[3]), int.Parse(data[4]));
                    _model.Shapes.Add(shape);
                }
            }
        }

        // upload shapes data to google drive
        public void UploadShapesFile()
        {
            SetUpGoogleDriveService();

            List<Google.Apis.Drive.v2.Data.File> files = _service.ListRootFileAndFolder();
            int fileIndex = files.FindIndex(item => item.Title == FILE_NAME);
            if (fileIndex > -1)
                _service.DeleteFile(files[fileIndex].Id);

            const string CONTENT_TYPE = "text/txt";
            _service.UploadFile(_filePath, CONTENT_TYPE);
        }
        
        // download shapes data from google drive
        public void DownloadShapesFile()
        {
            SetUpGoogleDriveService();

            List<Google.Apis.Drive.v2.Data.File> files = _service.ListRootFileAndFolder();
            int fileIndex = files.FindIndex(item => item.Title == FILE_NAME);
            if (fileIndex == -1)
                return;

            _service.DownloadFile(files[fileIndex], Directory.GetCurrentDirectory());
        }
    }
}
