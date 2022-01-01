using System;

namespace DrawingModel
{
    public class Rectangle : IShape
    {
        private readonly int _divider = 2;
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

        public string ShapeInfo
        {
            get
            {
                const string START = "Rectangle (";
                const string SEPARATOR = ", ";
                const string END = ")";
                return START + Math.Round(X1) + SEPARATOR + Math.Round(Y1) + SEPARATOR + Math.Round(X2) + SEPARATOR + Math.Round(Y2) + END;
            }
        }

        public double CenterX
        {
            get
            {
                return (X1 + X2) / _divider;
            }
        }

        public double CenterY
        {
            get
            {
                return (Y1 + Y2) / _divider;
            }
        }

        // draw rectangle on canvas
        public void Draw(IGraphics graphics)
        {
            graphics.DrawRectangle(X1, Y1, X2, Y2);
        }

        // draw rectangle selection on canvas
        public void DrawSelection(IGraphics graphics)
        {
            graphics.DrawSelection(X1, Y1, X2, Y2);
        }

        // check position in shape
        public bool IsPositionInShape(double posX, double posY)
        {
            return X1 <= posX && posX <= X2 && Y1 <= posY && posY <= Y2;
        }
    }
}