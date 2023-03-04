using CommonLib.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLib.Interfaces.Logger
{
    public interface IAppLogger
    {
        IInfoLogger InfoLogger { get; }

        IDebugLogger DebugLogger { get; set; }

        IErrorLogger ErrorLogger { get; set; }

        ISqlLogger SqlLogger { get; set; }

        TExecutionContext CreatExecutionContext(string data, bool force = false);
    }
}
