
namespace Caesura.Logging
{
    using System;
    using System.Collections.Concurrent;
    using Microsoft.Extensions.Logging;
    
    public class SolaceConsoleLoggerProvider : ILoggerProvider
    {
        private readonly SolaceConsoleLoggerConfiguration _config;
        private readonly ConcurrentDictionary<string, SolaceConsoleLogger> _loggers = new ConcurrentDictionary<string, SolaceConsoleLogger>();
        
        public SolaceConsoleLoggerProvider(SolaceConsoleLoggerConfiguration config)
        {
            _config = config;
        }
        
        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new SolaceConsoleLogger(name, _config));
        }
        
        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}
