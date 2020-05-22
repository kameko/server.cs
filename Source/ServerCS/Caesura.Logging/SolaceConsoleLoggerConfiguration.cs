
namespace Caesura.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Microsoft.Extensions.Logging;
    
    public class SolaceConsoleLoggerConfiguration
    {
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
        public Func<ISolaceConsoleLoggerFormatter> FormatterFactory
            { get; set; } = () => new SolaceConsoleLoggerFormatter();
        public CancellationToken Token
            { get; set; }
        public IConsoleTheme Theme
            { get; set; } = Themes.Native;
        public int InternalLoggerQueueOverloadThreshold
            { get; set; } = 1_000;
        public ObjectStringifyOption StringifyOption
            { get; set; } = ObjectStringifyOption.SerializeJsonRaw;
        
        public SolaceConsoleLoggerConfiguration()
        {
            
        }
        
        public SolaceConsoleLoggerConfiguration(SolaceConsoleLoggerConfiguration other)
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
        
        public SolaceConsoleLoggerConfiguration Clone()
        {
            return new SolaceConsoleLoggerConfiguration(this);
        }
        
        public enum ObjectStringifyOption
        {
            CallToString,
            SerializeJsonRaw,
            SerializeJsonPretty,
        }
    }
}
