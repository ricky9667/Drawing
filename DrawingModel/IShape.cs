namespace DrawingModel
{
    public interface IShape
    {
        ShapeType ShapeType
        {
            get; 
        }

        double X1
        {
            get; set;
        }

        double Y1
        {
            get; set;
        }

        double X2
        {
            get; set;
        }

        double Y2
        {
            get; set;
        }

        // draw shape on canvas
        void Draw(IGraphics graphics);
    }
}