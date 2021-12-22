namespace DrawingModel
{
    public class Line : IShape
    {
        public ShapeType ShapeType
        {
            get
            {
                return ShapeType.LINE;
            }
        }

        public double X1
        {
            get; set;
        }

        public double Y1
        {
            get; set;
        }

        public double X2
        {
            get; set;
        }

        public double Y2
        {
            get; set;
        }

        public IShape FirstShape
        {
            get; set;
        }

        public IShape SecondShape
        {
            get; set;
        }

        // draw line on canvas
        public void Draw(IGraphics graphics)
        {
            graphics.DrawLine(X1, Y1, X2, Y2);
        }

        // change line coordinate from shapes
        public void LocatePositionByShapes()
        {
            X1 = (FirstShape.X1 + FirstShape.X2) / 2;
            Y1 = (FirstShape.Y1 + FirstShape.Y2) / 2;
            X2 = (SecondShape.X1 + SecondShape.X2) / 2;
            Y2 = (SecondShape.Y1 + SecondShape.Y2) / 2;
        }
    }
}