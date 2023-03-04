using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLib.Interfaces
{
    /// <summary>
    /// Serilog Provider.
    /// </summary>
    public interface ISerilogProvider
    {
        /// <summary>
        /// Gets serilog instance (singleton).
        /// </summary>
        ILogger Logger { get; }
    }
}
