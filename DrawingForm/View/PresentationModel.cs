using DrawingModel;
using System.Windows.Forms;
using System.ComponentModel;

namespace DrawingForm.PresentationModel
{
    class PresentationModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Model _model;
        private bool _lineButtonEnabled = true;
        private bool _rectangleButtonEnabled = true;
        private bool _ellipseButtonEnabled = true;
        public PresentationModel(Model model, Control canvas)
        {
            _model = model;
        }

        // draw shapes on canvas
        public void Draw(System.Drawing.Graphics graphics)
        {
            _model.Draw(new WindowsFormsGraphicsAdaptor(graphics));
        }

        // notify binding properties changed
        private void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string SelectedShapeInfo
        {
            get
            {
                return _model.SelectedShapeInfo;
            }
        }

        public bool LineButtonEnabled
        {
            get
            {
                return _lineButtonEnabled;
            }
            set
            {
                _lineButtonEnabled = value;
                NotifyPropertyChanged(nameof(LineButtonEnabled));
            }
        }

        public bool RectangleButtonEnabled
        {
            get
            {
                return _rectangleButtonEnabled;
            }
            set
            {
                _rectangleButtonEnabled = value;
                NotifyPropertyChanged(nameof(RectangleButtonEnabled));
            }
        }

        public bool EllipseButtonEnabled
        {
            get
            {
                return _ellipseButtonEnabled;
            }
            set
            {
                _ellipseButtonEnabled = value;
                NotifyPropertyChanged(nameof(EllipseButtonEnabled));
            }
        }
    }
}