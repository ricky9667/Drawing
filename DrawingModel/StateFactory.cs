using System;

namespace DrawingModel
{
    public class StateFactory
    {
        // create state instance by shapeType
        public static IState CreateState(Model model, ShapeType shapeType)
        {
            switch (shapeType)
            {
                case ShapeType.LINE:
                    return new DrawingLineState(model);
                case ShapeType.RECTANGLE:
                    return new DrawingRectangleState(model);
                case ShapeType.ELLIPSE:
                    return new DrawingEllipseState(model);
                default:
                    return new PointerState(model);
            }
        }
    }
}
