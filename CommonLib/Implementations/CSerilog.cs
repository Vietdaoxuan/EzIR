using CommonLib.Interfaces;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace CommonLib.Implementations
{
    public class CSerilog : ISerilogProvider
    {
        public CSerilog(IConfiguration configuration, ILogger logger)
        {
            if (logger == null)
            {
                if (configuration == null)
                {
                    return;
                }

                Serilog.Core.Logger serilog = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();
                this.Logger = serilog;
            }
            else
            {
                this.Logger = logger;
            }
        }

        /// <inheritdoc/>
        public ILogger Logger { get; private set; }
    }
}
