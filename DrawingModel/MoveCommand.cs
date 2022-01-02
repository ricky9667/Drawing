using System;

namespace DrawingModel
{
    public class MoveCommand : ICommand
    {
        private readonly IShape _shape;
        private readonly Tuple<double, double> _offset;
        public MoveCommand(IShape shape, double offsetX, double offsetY)
        {
            _shape = shape;
            _offset = new Tuple<double, double>(offsetX, offsetY);
        }

        // execute move command
        public void Execute()
        {
            _shape.MoveShapeByOffset(_offset.Item1, _offset.Item2);
            _shape.UpdateSavedPosition();
        }

        // unexecute move command
        public void UndoExecute()
        {
            _shape.MoveShapeByOffset(-_offset.Item1, -_offset.Item2);
            _shape.UpdateSavedPosition();
        }
    }
}
