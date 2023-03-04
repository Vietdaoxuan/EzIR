using CommonLib.Model;
using System;

namespace CommonLib.Interfaces.Logger
{
    public interface IErrorLogger
    {
        void LogError(Exception exception);

        void LogErrorContext(Exception exception, TExecutionContext context);
    }
}
