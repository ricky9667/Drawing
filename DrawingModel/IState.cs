namespace DrawingModel
{
    public interface IState
    {
        ShapeType DrawingShape
        {
            get;
        }

        // handle pointer pressed
        void HandlePointerPressed(double posX, double posY);

        // handle pointer moved
        void HandlePointerMoved(double posX, double posY);

        // handle pointer released
        void HandlePointerReleased(double posX, double posY);
    }
}
