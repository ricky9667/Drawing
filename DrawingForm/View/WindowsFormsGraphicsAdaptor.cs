using System;
using System.Drawing;
using DrawingModel;

namespace DrawingForm.PresentationModel
{
    class WindowsFormsGraphicsAdaptor : IGraphics
    {
        private readonly Graphics _graphics;
        private readonly Pen _normalPen = new Pen(Color.Black, 2);
        private readonly Pen _selectionPen = new Pen(Color.Red, 3);
        private readonly Pen _cornerPen = new Pen(Color.Black, 1);

        public WindowsFormsGraphicsAdaptor(Graphics graphics)
        {
            _graphics = graphics;
            _selectionPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            _selectionPen.DashPattern = new float[] { 4F, 4F };
        }

        // clear drawings on canvas
        public void ClearAll()
        {
        }

        // draw line on canvas
        public void DrawLine(double x1, double y1, double x2, double y2)
        {
            _graphics.DrawLine(Pens.Black, (float)x1, (float)y1, (float)x2, (float)y2);
        }

        // draw rectangle on canvas
        public void DrawRectangle(double x1, double y1, double x2, double y2)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle((int)Math.Min(x1, x2), (int)Math.Min(y1, y2), (int)Math.Abs(x1 - x2), (int)Math.Abs(y1 - y2));
            _graphics.DrawRectangle(_normalPen, rectangle);
            _graphics.FillRectangle(new SolidBrush(Color.Yellow), rectangle);
        }

        // draw ellipse on canvas
        public void DrawEllipse(double x1, double y1, double x2, double y2)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle((int)Math.Min(x1, x2), (int)Math.Min(y1, y2), (int)Math.Abs(x1 - x2), (int)Math.Abs(y1 - y2));
            _graphics.DrawEllipse(_normalPen, rectangle);
            _graphics.FillEllipse(new SolidBrush(Color.Orange), rectangle);
        }

        // draw shape selection on canvas
        public void DrawSelection(double x1, double y1, double x2, double y2)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle((int)Math.Min(x1, x2), (int)Math.Min(y1, y2), (int)Math.Abs(x1 - x2), (int)Math.Abs(y1 - y2));
            _graphics.DrawRectangle(_selectionPen, rectangle);

            const int RADIUS = 3;
            DrawCornerCircle(x1, y1, RADIUS);
            DrawCornerCircle(x1, y2, RADIUS);
            DrawCornerCircle(x2, y1, RADIUS);
            DrawCornerCircle(x2, y2, RADIUS);
        }

        // draw corner circle of shape
        private void DrawCornerCircle(double posX, double posY, double radius)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle((int)(posX - radius), (int)(posY - radius), (int)(radius + radius), (int)(radius + radius));
            _graphics.DrawEllipse(_cornerPen, rectangle);
        }
    }
}