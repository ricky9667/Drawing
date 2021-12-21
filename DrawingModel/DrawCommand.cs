namespace DrawingModel
{
    class DrawCommand : ICommand
    {
        Model _model;
        IShape _shape;
        public DrawCommand(Model model, IShape shape)
        {
            _model = model;
            _shape = shape;
        }
        
        // execute draw command
        public void Execute()
        {
            _model.AddShape(_shape);
        }

        // unexecute draw command
        public void Unexecute()
        {
            _model.RemoveShape();
        }
    }
}
