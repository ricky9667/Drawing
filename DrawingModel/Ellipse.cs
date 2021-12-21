namespace DrawingModel
{
    public class Ellipse : IShape
    {
        public double X1 { get; set; }

        public double Y1 { get; set; }

        public double X2 { get; set; }

        public double Y2 { get; set; }

        // draw ellipse on canvas
        public void Draw(IGraphics graphics)
        {
            graphics.DrawEllipse(X1, Y1, X2, Y2);
        }
    }
}