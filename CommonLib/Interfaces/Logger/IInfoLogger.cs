using CommonLib.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLib.Interfaces.Logger
{
    public interface IInfoLogger
    {
        void LogInfo(string data);

        void LogInfoContext(TExecutionContext context, string data);
    }
}
