
namespace Standard.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Logging;
    using ConsoleLogger;
    
    public class LogItem
    {
        public ILoggerConfiguration Configuration { get; set; }
        public DateTime TimeStamp { get; set; }
        public LogLevel Level { get; set; }
        public EventId Id { get; set; }
        public string Name { get; set; }
        public object? State { get; set; }
        public Exception? Exception { get; set; }
        
        public LogItem(ILoggerConfiguration config, LogLevel logLevel, EventId eventId, string name, object? state, Exception? exception)
        {
            TimeStamp     = DateTime.UtcNow;
            Configuration = config;
            Level         = logLevel;
            Id            = eventId;
            Name          = name;
            State         = state;
            Exception     = exception;
        }
        
        public LogElement ToLogElement()
        {
            var statemsg = string.Empty;
            IEnumerable<LogElement.ItemElement> elms;
            if (State is LogState sls)
            {
                statemsg = sls.Message;
                elms = sls.Values.Select(x => 
                    new LogElement.ItemElement()
                    {
                        Position = x.Position,
                        Value = x.Value?.ToString() ?? string.Empty
                    });
            }
            else
            {
                elms = new List<LogElement.ItemElement>();
                statemsg = State?.ToString() ?? string.Empty;
            }
            
            var le = new LogElement()
            {
                TimeStamp     = this.TimeStamp,
                Level         = this.Level,
                EventId       = this.Id.Id,
                SenderService = this.Name,
                Message       = statemsg,
                Elements      = elms,
                Exception     = new LogElement.ExceptionElement(this.Exception),
            };
            return le;
        }
        
        public static implicit operator LogElement (LogItem item)
        {
            return item.ToLogElement();
        }
        
        public override string ToString()
        {
            return 
                $"[{Level}][{Name}({Id})]: {State}{(State is null ? string.Empty : " ")} " + 
                $"{(Exception is null ? string.Empty : Environment.NewLine)}{Exception}";
        }
    }
}
