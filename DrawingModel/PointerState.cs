namespace DrawingModel
{
    public class PointerState : IState
    {
        private readonly Model _model;
        public PointerState(Model model)
        {
            _model = model;
        }

        public ShapeType DrawingShape
        {
            get
            {
                return ShapeType.NULL;
            }
        }

        // handle pointer pressed
        public void HandlePointerPressed(double posX, double posY)
        {
            _model.SetFirstPointCoordinates(posX, posY);
            _model.UpdateSelectedShapeIndex(posX, posY);
        }

        // handle pointer moved
        public void HandlePointerMoved(double posX, double posY)
        {
            _model.UpdateSelectedShapeCoordinates(posX, posY);
        }

        // handle pointer released
        public void HandlePointerReleased(double posX, double posY)
        {
            _model.MoveShape(posX, posY);
        }
    }
}
