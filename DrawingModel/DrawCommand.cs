namespace DrawingModel
{
    public class DrawCommand : ICommand
    {
        private readonly Model _model;
        private readonly IShape _shape;
        private readonly int _index;
        public DrawCommand(Model model, IShape shape)
        {
            _model = model;
            _shape = shape;
            _index = _model.Shapes.Count;
        }
        
        // execute draw command
        public void Execute()
        {
            _model.Shapes.Add(_shape);
        }

        // unexecute draw command
        public void UndoExecute()
        {
            _model.Shapes.RemoveAt(_index);
        }
    }
}
