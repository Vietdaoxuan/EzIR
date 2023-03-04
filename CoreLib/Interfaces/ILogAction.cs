using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib.Interfaces
{
    public interface ILogAction
    {
        string SessionID { get; set; }

        long UserID { get; set; }

        string Action { get; set; }

        string SQL { get; set; }

        string UserAgent { get; set; }

        string ServerIP { get; set; }

        string ClientIP { get; set; }

        DateTime DateCreate { get; set; }
    }
}
