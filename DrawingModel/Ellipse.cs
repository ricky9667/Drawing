namespace DrawingModel
{
    public class Ellipse : IShape
    {
        public ShapeType ShapeType
        {
            get
            {
                return ShapeType.ELLIPSE;
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

        public double CenterX
        {
            get
            {
                return (X1 + X2) / 2;
            }
        }

        public double CenterY
        {
            get
            {
                return (Y1 + Y2) / 2;
            }
        }

        // draw ellipse on canvas
        public void Draw(IGraphics graphics)
        {
            graphics.DrawEllipse(X1, Y1, X2, Y2);
        }

        // check position in shape
        public bool IsPositionInShape(double posX, double posY)
        {
            return (X1 <= posX && posX <= X2 && Y1 <= posY && posY <= Y2);
        }
    }
}