using System.Collections.Generic;

namespace DrawingModel
{
    public class ClearCommand : ICommand
    {
        private readonly Model _model;
        private readonly List<IShape> _shapes = new List<IShape>();
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
        public void UndoExecute()
        {
            foreach (IShape shape in _shapes)
            {
                _model.Shapes.Add(shape);
            }
        }
    }
}
