using CommonLib.Interfaces.Logger;
using CommonLib.Model;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLib.Implementations.Logger
{
    public class DebugLogger : BaseLogger, IDebugLogger
    {
        private const string _debugLogTemplate = @"=================
        EId     = {0} ({1}) [{2}]
        Source  = {3}
        Data    = {4}";

        private const string _contextTemplate = @"=================
        Source  = {0}
        EId     = {1} ({2}) [{3}] BeginF
        {4}EId     = {5} ({6}) [{7}] EndF <<{8}>>
        Finally = {9}";

        public ILogger Logger { get; }

        public void LogDebug(string data)
        {
            string eId = Guid.NewGuid().ToString(); // execution id => chi xuat hien 1 lan execute đó, lần execute sau là id khac
            this.Logger.Debug(
                    _debugLogTemplate,
                    eId,
                    ThreadId,
                    TaskId,
                    GetDeepCaller(),
                    data);
        }

        public void LogDebugContext(TExecutionContext context, string data)
        {
            this.Logger.Debug(
                _contextTemplate,
                GetDeepCaller(),
                context.Id,
                context.ThreadId,
                context.TaskId,
                context.Buffer,
                context.Id,
                context.ThreadId,
                context.TaskId,
                context.ElapsedMilliseconds,
                data);
        }

        public DebugLogger(ILogger logger)
            : base(logger)
        {
        }
    }
}
