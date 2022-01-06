using System;

namespace DrawingModel
{
    public enum ShapeType
    {
        NULL,
        LINE,
        RECTANGLE,
        ELLIPSE
    }

    public class ShapeFactory
    {
        // create shape instance by shapeType
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
                    const string MESSAGE = "ShapeType is null or does not exist.";
                    throw new Exception(MESSAGE);
            }
        }

        // create shape instance by shapeName
        public static IShape CreateShape(string shapeName)
        {
            switch (shapeName)
            {
                case nameof(ShapeType.LINE):
                    return new Line();
                case nameof(ShapeType.RECTANGLE):
                    return new Rectangle();
                case nameof(ShapeType.ELLIPSE):
                    return new Ellipse();
                default:
                    const string MESSAGE = "ShapeType is null or does not exist.";
                    throw new Exception(MESSAGE);
            }
        }
    }
}
