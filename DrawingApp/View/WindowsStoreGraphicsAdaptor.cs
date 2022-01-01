using System;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using DrawingModel;

namespace DrawingApp.PresentationModel
{
    class WindowsStoreGraphicsAdaptor : IGraphics
    {
        private readonly Canvas _canvas;

        public WindowsStoreGraphicsAdaptor(Canvas canvas)
        {
            _canvas = canvas;
        }

        // clear drawings on canvas
        public void ClearAll()
        {
            _canvas.Children.Clear();
        }

        // draw line on canvas
        public void DrawLine(double x1, double y1, double x2, double y2)
        {
            Windows.UI.Xaml.Shapes.Line line = new Windows.UI.Xaml.Shapes.Line();
            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;
            line.Stroke = new SolidColorBrush(Colors.Black);
            _canvas.Children.Add(line);
        }

        // draw rectangle on canvas
        public void DrawRectangle(double x1, double y1, double x2, double y2)
        {
            Windows.UI.Xaml.Shapes.Rectangle rectangle = new Windows.UI.Xaml.Shapes.Rectangle();

            rectangle.Margin = new Windows.UI.Xaml.Thickness(Math.Min(x1, x2), Math.Min(y1, y2), 0, 0);
            rectangle.Width = Math.Abs(x1 - x2);
            rectangle.Height = Math.Abs(y1 - y2);
            rectangle.Stroke = new SolidColorBrush(Colors.Black);
            rectangle.Fill = new SolidColorBrush(Colors.Yellow);
            rectangle.StrokeThickness = 2F;

            _canvas.Children.Add(rectangle);
        }

        // draw ellipse on canvas
        public void DrawEllipse(double x1, double y1, double x2, double y2)
        {
            Windows.UI.Xaml.Shapes.Ellipse ellipse = new Windows.UI.Xaml.Shapes.Ellipse();
            ellipse.Margin = new Windows.UI.Xaml.Thickness(Math.Min(x1, x2), Math.Min(y1, y2), 0, 0);
            ellipse.Width = Math.Abs(x1 - x2);
            ellipse.Height = Math.Abs(y1 - y2);
            ellipse.Stroke = new SolidColorBrush(Colors.Black);
            ellipse.Fill = new SolidColorBrush(Colors.Orange);
            ellipse.StrokeThickness = 2F;
            _canvas.Children.Add(ellipse);
        }

        // draw shape selection on canvas
        public void DrawSelection(double x1, double y1, double x2, double y2)
        {
            Windows.UI.Xaml.Shapes.Rectangle rectangle = new Windows.UI.Xaml.Shapes.Rectangle();
            rectangle.Margin = new Windows.UI.Xaml.Thickness(Math.Min(x1, x2), Math.Min(y1, y2), 0, 0);
            rectangle.Width = Math.Abs(x1 - x2);
            rectangle.Height = Math.Abs(y1 - y2);
            rectangle.Stroke = new SolidColorBrush(Colors.Red);
            rectangle.StrokeDashArray = new DoubleCollection();
            rectangle.StrokeDashArray.Add(4F);
            rectangle.StrokeThickness = 4F;
            _canvas.Children.Add(rectangle);

            const int RADIUS = 4;
            DrawCornerCircle(x1, y1, RADIUS);
            DrawCornerCircle(x1, y2, RADIUS);
            DrawCornerCircle(x2, y1, RADIUS);
            DrawCornerCircle(x2, y2, RADIUS);
        }

        // draw corner circle of shape
        private void DrawCornerCircle(double posX, double posY, double radius)
        {
            Windows.UI.Xaml.Shapes.Ellipse ellipse = new Windows.UI.Xaml.Shapes.Ellipse();
            ellipse.Margin = new Windows.UI.Xaml.Thickness((int)(posX - radius), (int)(posY - radius), 0, 0);
            ellipse.Width = Math.Abs((int)(radius + radius));
            ellipse.Height = Math.Abs((int)(radius + radius));
            ellipse.Stroke = new SolidColorBrush(Colors.Black);
            _canvas.Children.Add(ellipse);
        }
    }
}