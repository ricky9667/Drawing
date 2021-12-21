using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingModel
{
    public enum ShapeType
    {
        LINE,
        RECTANGLE,
        ELLIPSE
    }

    public class ShapeFactory
    {
        // use factory pattern to create shape instance
        public static IShape CreateShape(ShapeType shapeType)
        {
            switch (shapeType)
            {
                case ShapeType.LINE:
                    return new Line();
                case ShapeType.RECTANGLE:
                    return new Rectangle();
                case ShapeType.ELLIPSE:
                    return new Ellipse();
                default:
                    throw new Exception("ShapeType is null or does not exist.");
            }
        }
    }
}
