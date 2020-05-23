
namespace Caesura.Logging.ConsoleLogger
{
    using System;
    using System.Collections.Concurrent;
    using Microsoft.Extensions.Logging;
    
    public class ConsoleLoggerProvider : ILoggerProvider
    {
        private readonly ConsoleLoggerConfiguration _config;
        private readonly ConcurrentDictionary<string, ConsoleLogger> _loggers = new ConcurrentDictionary<string, ConsoleLogger>();
        
        public ConsoleLoggerProvider(ConsoleLoggerConfiguration config)
        {
            _config = config;
        }
        
        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new ConsoleLogger(name, _config));
        }
        
        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}
