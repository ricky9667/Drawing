using DrawingModel;
using System.Windows.Forms;
using System.ComponentModel;

namespace DrawingForm.PresentationModel
{
    class PresentationModel
    {
        private readonly Model _model;
        public PresentationModel(Model model, Control canvas)
        {
            _model = model;
        }

        // draw shapes on canvas
        public void Draw(System.Drawing.Graphics graphics)
        {
            _model.Draw(new WindowsFormsGraphicsAdaptor(graphics));
        }

        public string SelectedShapeInfo
        {
            get
            {
                return _model.SelectedShapeInfo;
            }
        }
    }
}