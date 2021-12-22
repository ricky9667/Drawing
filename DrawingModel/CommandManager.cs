using System;
using System.Collections.Generic;

namespace DrawingModel
{
    public class CommandManager
    {
        private readonly Stack<ICommand> _undoCommands = new Stack<ICommand>();
        private readonly Stack<ICommand> _redoCommands = new Stack<ICommand>();

        // execute new command
        public void RunCommand(ICommand command)
        {
            command.Execute();
            _undoCommands.Push(command);
            _redoCommands.Clear();
        }

        // unexecute command
        public void Undo()
        {
            if (_undoCommands.Count <= 0)
            {
                const string MESSAGE = "No commands in stack";
                throw new Exception(MESSAGE);
            }

            ICommand command = _undoCommands.Pop();
            command.UndoExecute();
            _redoCommands.Push(command);
        }

        // execute redo command
        public void Redo()
        {
            if (_redoCommands.Count <= 0)
            {
                const string MESSAGE = "No commands in stack to redo";
                throw new Exception(MESSAGE);
            }
            
            ICommand command = _redoCommands.Pop();
            command.Execute();
            _undoCommands.Push(command);
        }

        public bool CanUndo
        {
            get
            {
                return _undoCommands.Count > 0;
            }
        }

        public bool CanRedo
        {
            get
            {
                return _redoCommands.Count > 0;
            }
        }
    }
}
