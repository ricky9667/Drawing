﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
