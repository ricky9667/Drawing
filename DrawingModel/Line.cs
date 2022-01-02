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

        // update coordinates to saved position
        public void UpdateSavedPosition()
        {
            X1 = FirstShape.CenterX;
            Y1 = FirstShape.CenterY;
            X2 = SecondShape.CenterX;
            Y2 = SecondShape.CenterY;
        }

        // move shape
        public void MoveShapeByOffset(double offsetX, double offsetY)
        {
            
        }

        // check position in shape
        public bool IsPositionInShape(double posX, double posY)
        {
            bool xInRange = (X1 < X2) ? (X1 <= posX && posX <= X2) : (X2 <= posX && posX <= X1);
            bool yInRange = (Y1 < Y2) ? (Y1 <= posY && posY <= Y2) : (Y2 <= posY && posY <= Y1);
            return xInRange && yInRange;
        }
    }
}