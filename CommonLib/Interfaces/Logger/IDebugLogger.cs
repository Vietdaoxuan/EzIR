using CommonLib.Model;

namespace CommonLib.Interfaces.Logger
{
    public interface IDebugLogger
    {
        void LogDebug(string data);

        void LogDebugContext(TExecutionContext context, string data);
    }
}
