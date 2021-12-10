using System;
using System.Drawing;
using DrawingModel;

namespace DrawingForm.PresentationModel
{
    class WindowsFormsGraphicsAdaptor : IGraphics
    {
        private readonly Graphics _graphics;
        private readonly Pen _pen = Pens.Black;

        public WindowsFormsGraphicsAdaptor(Graphics graphics)
        {
            _graphics = graphics;
        }

        // clear drawings on canvas
        public void ClearAll()
        {
        }

        // draw line on canvas
        public void DrawLine(double x1, double y1, double x2, double y2)
        {
            _graphics.DrawLine(_pen, (float)x1, (float)y1, (float)x2, (float)y2);
        }

        // draw rectangle on canvas
        public void DrawRectangle(double x1, double y1, double x2, double y2)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle((int)Math.Min(x1, x2), (int)Math.Min(y1, y2), (int)Math.Abs(x1 - x2), (int)Math.Abs(y1 - y2));
            _graphics.DrawRectangle(_pen, rectangle);
            _graphics.FillRectangle(new SolidBrush(Color.Yellow), rectangle);
        }

        // draw ellipse on canvas
        public void DrawEllipse(double x1, double y1, double x2, double y2)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle((int)Math.Min(x1, x2), (int)Math.Min(y1, y2), (int)Math.Abs(x1 - x2), (int)Math.Abs(y1 - y2));
            _graphics.DrawEllipse(_pen, rectangle);
            _graphics.FillEllipse(new SolidBrush(Color.Orange), rectangle);
        }
    }
}