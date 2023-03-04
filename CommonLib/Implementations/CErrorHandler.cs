using CommonLib.Interfaces;
using CoreLib.Interfaces;
using Serilog;
using System;


namespace CommonLib.Implementations
{
    public class CErrorHandler : IErrorHandler
    {
        private readonly ILogger _logger;

        public CErrorHandler(ISerilogProvider serilogProvider)
        {
            this._logger = serilogProvider.Logger;
        }

        public void WriteToFile(Exception ex)
        {
            string template = "\r\n-----Message-----\r\n{0}\r\n-----Source ---\r\n{1}\r\n-----StackTrace ---\r\n{2}\r\n-----TargetSite ---\r\n{3}";
            this._logger.Error(template, ex.Message, ex.Source, ex.StackTrace, ex.TargetSite);
        }

        public void WriteStringToFile(string SPname, string paramArr)
        {
            string template = "\r\n-----SPname-----\r\n{0}\r\n-----paramArr ---\r\n{1}\r\n";
            this._logger.Error(template, SPname, paramArr);
        }
    }
}
