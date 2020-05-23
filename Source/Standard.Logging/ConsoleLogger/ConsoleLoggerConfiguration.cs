
namespace Standard.Logging.ConsoleLogger
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Microsoft.Extensions.Logging;
    
    public class ConsoleLoggerConfiguration : ILoggerConfiguration
    {
        public static readonly ConsoleLoggerConfiguration Default = new ConsoleLoggerConfiguration();
        
        public LogLevel LogLevel
            { get; set; } = LogLevel.Debug;
        public int EventId
            { get; set; } = 0;
        public string TimeStampFormat
            { get; set; } = "dddd H:mm:ss.fff"; // or "dd/MM/yyyy"
        public IEnumerable<string> TrimNames
            { get; set; } = new List<string>()
            {
                "Caesura.Solace."
            };
        public IDictionary<string, string> ReplaceNames
            { get; set; } = new Dictionary<string, string>()
            {
                { "Microsoft.Hosting.Lifetime", "System" }
            };
        public Func<IConsoleLoggerFormatter> FormatterFactory
            { get; set; } = () => new ConsoleLoggerFormatter();
        public CancellationToken Token
            { get; set; }
        public IConsoleTheme Theme
            { get; set; } = Themes.Native;
        public int InternalLoggerQueueOverloadThreshold
            { get; set; } = 1_000;
        public ObjectStringifyOption StringifyOption
            { get; set; } = ObjectStringifyOption.SerializeJsonRaw;
        
        public ConsoleLoggerConfiguration()
        {
            
        }
        
        public ConsoleLoggerConfiguration(ConsoleLoggerConfiguration other)
        {
            LogLevel                             = other.LogLevel;
            EventId                              = other.EventId;
            TimeStampFormat                      = other.TimeStampFormat;
            TrimNames                            = new List<string>(other.TrimNames);
            ReplaceNames                         = new Dictionary<string, string>(other.ReplaceNames);
            FormatterFactory                     = other.FormatterFactory;
            Token                                = other.Token;
            Theme                                = other.Theme;
            InternalLoggerQueueOverloadThreshold = other.InternalLoggerQueueOverloadThreshold;
            StringifyOption                      = other.StringifyOption;
        }
        
        public ConsoleLoggerConfiguration Clone()
        {
            return new ConsoleLoggerConfiguration(this);
        }
        
        public enum ObjectStringifyOption
        {
            CallToString,
            SerializeJsonRaw,
            SerializeJsonPretty,
        }
    }
}
