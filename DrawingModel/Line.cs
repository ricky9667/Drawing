using System;

namespace DrawingModel
{
    public class Line : IShape
    {
        private readonly int _divider = 2;
        public ShapeType ShapeType
        {
            get
            {
                return ShapeType.LINE;
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
                const string START = "Line (";
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

        public IShape FirstShape
        {
            get; set;
        }

        public IShape SecondShape
        {
            get; set;
        }

        // draw line on canvas
        public void Draw(IGraphics graphics)
        {
            graphics.DrawLine(X1, Y1, X2, Y2);
        }

        // draw line selection on canvas
        public void DrawSelection(IGraphics graphics)
        {
            graphics.DrawSelection(X1, Y1, X2, Y2);
        }

        // change line coordinate from shapes
        public void LocatePositionByShapes()
        {
            X1 = FirstShape.CenterX;
            Y1 = FirstShape.CenterY;
            X2 = SecondShape.CenterX;
            Y2 = SecondShape.CenterY;
        }

        // check position in shape
        public bool IsPositionInShape(double posX, double posY)
        {
            return X1 <= posX && posX <= X2 && Y1 <= posY && posY <= Y2;
        }
    }
}