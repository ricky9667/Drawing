using System;

namespace DrawingModel
{
    public class MoveCommand : ICommand
    {
        private readonly Model _model;
        private readonly IShape _shape;
        private readonly Tuple<double, double> _offset;
        public MoveCommand(Model model, IShape shape, double offsetX, double offsetY)
        {
            _model = model;
            _shape = shape;
            _offset = new Tuple<double, double>(offsetX, offsetY);
        }

        // execute move command
        public void Execute()
        {
            _shape.MoveShapeByOffset(_offset.Item1, _offset.Item2);
            _shape.UpdateSavedPosition();
            _model.UpdateLinesPosition();
        }

        // unexecute move command
        public void UndoExecute()
        {
            _shape.MoveShapeByOffset(-_offset.Item1, -_offset.Item2);
            _shape.UpdateSavedPosition();
            _model.UpdateLinesPosition();
        }
    }
}
