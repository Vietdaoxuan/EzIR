using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonLib.Implementations.Logger
{
    public abstract class BaseLogger
    {
        public ILogger Logger { get; }

        public int ThreadId => Thread.CurrentThread.ManagedThreadId; // thread info

        public int TaskId => Task.CurrentId == null ? 0 : Convert.ToInt32(Task.CurrentId); // task info

        protected BaseLogger(ILogger logger)
        {
            Logger = logger;
        }

        /// <summary>
        /// ke thua tu 5G: xac dinh vi tri function dang run
        /// </summary>
        /// <returns></returns>
        protected static string GetDeepCaller()
        {
            var strCallerName = string.Empty;
            for (var i = 3; i >= 3; i--)
                strCallerName += GetCaller(i);

            // returns a composite of the namespace, class and method name.
            return strCallerName;
        }

        /// <summary> 
        /// ke thua tu 5G: xac dinh vi tri function dang run
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        private static string GetCaller(int level = 2)
        {
            var m = new StackTrace().GetFrame(level).GetMethod();

            if (m.DeclaringType == null) return string.Empty; 

            // .Name is the name only, .FullName includes the namespace
            var className = m.DeclaringType.FullName;

            // the method/function name you are looking for.
            var methodName = m.Name;

            // temp
            var temp = className + "->" + methodName;

            // returns a composite of the namespace, class and method name.
            return temp;
        }
    }
}
