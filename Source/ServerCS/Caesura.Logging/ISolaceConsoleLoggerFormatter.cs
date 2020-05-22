
namespace Caesura.Logging
{
    public interface ISolaceConsoleLoggerFormatter
    {
        void PreLog(LogItem item);
        void PostLog(LogItem item);
        string Format(LogItem item);
    }
}
