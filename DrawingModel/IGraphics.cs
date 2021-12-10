namespace DrawingModel
{
    public interface IGraphics
    {
        // clear drawings on canvas
        void ClearAll();

        // draw line on canvas
        void DrawLine(double x1, double y1, double x2, double y2);

        // draw rectangle on canvas
        void DrawRectangle(double x1, double y1, double x2, double y2);

        // draw ellipse on canvas
        void DrawEllipse(double x1, double y1, double x2, double y2);
    }
}