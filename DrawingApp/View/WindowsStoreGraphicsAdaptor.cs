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
            rectangle.StrokeThickness = 4F;

            _canvas.Children.Add(rectangle);
        }
    }
}