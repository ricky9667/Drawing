﻿using System;

namespace DrawingModel
{
    public enum ShapeType
    {
        LINE,
        RECTANGLE,
        ELLIPSE,
        NULL
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
                    const string MESSAGE = "ShapeType is null or does not exist.";
                    throw new Exception(MESSAGE);
            }
        }
    }
}
