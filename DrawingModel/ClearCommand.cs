using System.Collections.Generic;

namespace DrawingModel
{
    class ClearCommand : ICommand
    {
        Model _model;
        List<IShape> _shapes = new List<IShape>();
        public ClearCommand(Model model)
        {
            _model = model;
            foreach (IShape shape in model.Shapes)
            {
                _shapes.Add(shape);
            }
        }

        // clear shapes
        public void Execute()
        {
            _model.Shapes.Clear();
        }

        // undo clear shapes
        public void Unexecute()
        {
            foreach (IShape shape in _shapes)
            {
                _model.Shapes.Add(shape);
            }
        }
    }
}
