namespace DrawingModel
{
    public interface ICommand
    {
        // execute command
        void Execute();

        // undo command
        void Unexecute();
    }
}
