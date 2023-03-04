using CommonLib.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLib.Interfaces.Logger
{
    public interface ISqlLogger
    {
        void LogSql(string data);

        void LogSqlContext(TExecutionContext context, string data);
    }
}
