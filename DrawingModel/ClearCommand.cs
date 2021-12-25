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
            _shapes.AddRange(model.Shapes);
        }

        // clear shapes
        public void Execute()
        {
            _model.Shapes.Clear();
        }

        // undo clear shapes
        public void UndoExecute()
        {
            _model.Shapes.AddRange(_shapes);
        }
    }
}
