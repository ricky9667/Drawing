namespace DrawingModel
{
    public class DrawingLineState : IState
    {
        private readonly Model _model;
        public DrawingLineState(Model model)
        {
            _model = model;
        }

        public ShapeType DrawingShape
        {
            get
            {
                return ShapeType.LINE;
            }
        }

        // handle pointer pressed
        public void HandlePointerPressed(double posX, double posY)
        {
            _model.SetFirstPointCoordinates(posX, posY);
            _model.SetHintFirstPointCoordinates(posX, posY);
        }

        // handle pointer moved
        public void HandlePointerMoved(double posX, double posY)
        {
            _model.SetHintSecondPointCoordinates(posX, posY);
        }

        // handle pointer released
        public void HandlePointerReleased(double posX, double posY)
        {
            _model.AddNewLine(posX, posY);
        }
    }
}
