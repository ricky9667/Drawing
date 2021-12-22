namespace DrawingModel
{
    public class Rectangle : IShape
    {
        public ShapeType ShapeType
        {
            get
            {
                return ShapeType.RECTANGLE;
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

        // draw rectangle on canvas
        public void Draw(IGraphics graphics)
        {
            graphics.DrawRectangle(X1, Y1, X2, Y2);
        }
    }
}