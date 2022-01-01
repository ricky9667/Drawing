using Windows.UI.Xaml.Controls;
using DrawingModel;

namespace DrawingApp.PresentationModel
{
    class PresentationModel
    {
        private readonly Model _model;
        private readonly IGraphics _graphics;
        public PresentationModel(Model model, Canvas canvas)
        {
            _model = model;
            _graphics = new WindowsStoreGraphicsAdaptor(canvas);
        }

        // draw shapes on canvas
        public void Draw()
        {
            // 重複使用igraphics物件
            _model.Draw(_graphics);
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