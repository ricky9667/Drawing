namespace DrawingModel
{
    public class DrawingRectangleState : IState  
    {
        private readonly Model _model;
        public DrawingRectangleState(Model model)
        {
            _model = model;
        }

        public ShapeType DrawingShape
        {
            get
            {
                return ShapeType.RECTANGLE;
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
            _model.AddNewShape(posX, posY);
        }
    }
}
