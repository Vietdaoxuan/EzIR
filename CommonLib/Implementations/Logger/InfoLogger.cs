using CommonLib.Interfaces.Logger;
using CommonLib.Model;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLib.Implementations.Logger
{
    public class InfoLogger : BaseLogger, IInfoLogger
    {
        private const string _infoLogTemplate = @"=================
        Source  = {0}
        Data    = {1}";

        private const string _contextTemplate = @"{0} = {1} ({2}) [{3}] <<{4}>> => {5}";

        public InfoLogger(ILogger logger)
            : base(logger)
        {
        }

        public void LogInfo(string data)
        {
            Logger.Information(_infoLogTemplate, GetDeepCaller(), data);
        }

        public void LogInfoContext(TExecutionContext context, string data)
        {
            Logger.Information(
                _contextTemplate,
                GetDeepCaller(),
                data,
                ThreadId,
                TaskId,
                context.ElapsedMilliseconds,
                context.Id);
        }
    }
}
