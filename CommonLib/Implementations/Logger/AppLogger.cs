using CommonLib.Interfaces.Logger;
using CommonLib.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonLib.Implementations.Logger
{
    public class AppLogger : IAppLogger
    {
        public IInfoLogger InfoLogger { get; }

        public IDebugLogger DebugLogger { get; set; }

        public IErrorLogger ErrorLogger { get; set; }

        public ISqlLogger SqlLogger { get; set; }

        public string Timestamp => DateTime.Now.ToString("yyyyMMddHHmmssfff"); // moc time exec code

        public int ThreadId => Thread.CurrentThread.ManagedThreadId; // thread info

        public string ThreadIdS => ThreadId.ToString("0000"); // thread info

        public int TaskId => Task.CurrentId == null ? 0 : Convert.ToInt32(Task.CurrentId); // task info

        public string TaskIdS => TaskId.ToString("000000"); // task info

        /// <summary>
        /// // func info
        /// </summary>
        public string FunctionName => new StackTrace().GetFrame(2).GetMethod().Name;

        public AppLogger(IInfoLogger infoLogger, IDebugLogger debugLogger, IErrorLogger errorLogger, ISqlLogger sqlLogger)
        {
            InfoLogger = infoLogger;
            DebugLogger = debugLogger;
            ErrorLogger = errorLogger;
            SqlLogger = sqlLogger;
        }

        public TExecutionContext CreatExecutionContext(string data, bool force = false)
        {
            // tao ec object
            var executionContext = new TExecutionContext()
            {
                Id = Guid.NewGuid().ToString(), // execution id => chi xuat hien 1 lan execute đó, lần execute sau là id khac
                Data = data,
                Timestamp = Timestamp,
                ThreadId = ThreadId,
                ThreadIdS = ThreadIdS,
                TaskId = TaskId,
                TaskIdS = TaskIdS,
                FunctionName = FunctionName,
            };

            return executionContext;
        }
    }
}
