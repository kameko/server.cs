
namespace Standard.Logging
{
    public interface IConsoleLoggerFormatter
    {
        void PreLog(LogItem item);
        void PostLog(LogItem item);
        string Format(LogItem item);
    }
}
