using CommonLib.Interfaces.Logger;
using CommonLib.Model;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLib.Implementations.Logger
{
    public class SqlLogger : BaseLogger, ISqlLogger
    {
        private const string _sqlLogTemplate = @"=================
            Source  = {0}
            Data    = {1}";

        private const string _contextTemplate = @"=================
            Source  = {0} => {1} ({2}) [{3}]
            Data    = {4}";

        public void LogSql(string data)
        {
            this.Logger.Information(_sqlLogTemplate, GetDeepCaller(), data);
        }

        public void LogSqlContext(TExecutionContext context, string data)
        {
            this.Logger.Information(
                _contextTemplate,
                GetDeepCaller(),
                context.Id,
                ThreadId,
                TaskId,
                data);
        }

        public SqlLogger(ILogger logger)
            : base(logger)
        {
        }
    }
}
