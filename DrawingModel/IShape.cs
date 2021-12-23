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

        double CenterX
        {
            get;
        }

        double CenterY
        {
            get;
        }

        bool IsSelected
        {
            get; set;
        }

        // draw shape on canvas
        void Draw(IGraphics graphics);

        // draw shape selection on canvas
        void DrawSelection(IGraphics graphics);

        // check position in shape
        bool IsPositionInShape(double posX, double posY);
    }
}