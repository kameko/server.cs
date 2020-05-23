
namespace Caesura.Logging
{
    using System;
    using System.Runtime.CompilerServices;
    using Microsoft.Extensions.Logging;
    using ConsoleLogger;
    
    public static class LoggerExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InstanceAbreaction(this ILogger logger)
        {
            logger.Trace("Instance created.");
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EnterMethod(this ILogger logger, string method_name)
        {
            logger.Trace($"Entering {method_name}.");
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EnterMethod(this ILogger logger, string method_name, string message, params object[] args)
        {
            logger.Trace($"Entering {method_name} " + message, args);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ExitMethod(this ILogger logger, string method_name)
        {
            logger.Trace($"Exiting {method_name}.");
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ExitMethod(this ILogger logger, string method_name, string message, params object[] args)
        {
            logger.Trace($"Exiting {method_name} " + message, args);
        }
        
        public static ILoggingBuilder AddCaesuraConsoleLogger(this ILoggingBuilder builder)
        {
            AddCaesuraConsoleLogger(builder, null);
            return builder;
        }
        
        public static ILoggingBuilder AddCaesuraConsoleLogger(this ILoggingBuilder builder, LogLevel log_level)
        {
            var config = new ConsoleLoggerConfiguration()
            {
                LogLevel = log_level,
            };
            
            var provider = new ConsoleLoggerProvider(config);
            builder.AddProvider(provider);
            
            return builder;
        }
        
        public static ILoggingBuilder AddCaesuraConsoleLogger(this ILoggingBuilder builder, Action<ConsoleLoggerConfiguration>? configure)
        {
            var config = new ConsoleLoggerConfiguration();
            configure?.Invoke(config);
            
            var provider = new ConsoleLoggerProvider(config);
            builder.AddProvider(provider);
            
            return builder;
        }
        
        public static void Information(this ILogger logger, string message, params object[] args)
        {
            RawLog(LogLevel.Information, logger, null, message, args);
        }
        
        public static void Information(this ILogger logger, Exception exception, string message, params object[] args)
        {
            RawLog(LogLevel.Information, logger, exception, message, args);
        }
        
        public static void Warning(this ILogger logger, string message, params object[] args)
        {
            RawLog(LogLevel.Warning, logger, null, message, args);
        }
        
        public static void Warning(this ILogger logger, Exception exception, string message, params object[] args)
        {
            RawLog(LogLevel.Warning, logger, exception, message, args);
        }
        
        public static void Error(this ILogger logger, string message, params object[] args)
        {
            RawLog(LogLevel.Error, logger, null, message, args);
        }
        
        public static void Error(this ILogger logger, Exception exception, string message, params object[] args)
        {
            RawLog(LogLevel.Error, logger, exception, message, args);
        }
        
        public static void Critical(this ILogger logger, string message, params object[] args)
        {
            RawLog(LogLevel.Critical, logger, null, message, args);
        }
        
        public static void Critical(this ILogger logger, Exception exception, string message, params object[] args)
        {
            RawLog(LogLevel.Critical, logger, exception, message, args);
        }
        
        public static void Debug(this ILogger logger, string message, params object[] args)
        {
            RawLog(LogLevel.Debug, logger, null, message, args);
        }
        
        public static void Debug(this ILogger logger, Exception exception, string message, params object[] args)
        {
            RawLog(LogLevel.Debug, logger, exception, message, args);
        }
        
        public static void Trace(this ILogger logger, string message, params object[] args)
        {
            RawLog(LogLevel.Trace, logger, null, message, args);
        }
        
        public static void Trace(this ILogger logger, Exception exception, string message, params object[] args)
        {
            RawLog(LogLevel.Trace, logger, exception, message, args);
        }
        
        public static void Raw(this ILogger logger, LogLevel level, Exception? exception, string message, params object[] args)
        {
            RawLog(level, logger, exception, message, args);
        }
        
        private static void RawLog(LogLevel level, ILogger logger, Exception? exception, string message, params object[] args)
        {
            logger.Log<LogState>(level, GetId(logger), LogState.Create(message, args), exception, Formatter);
        }
        
        private static EventId GetId(ILogger logger)
        {
            if (logger is ConsoleLogger.ConsoleLogger csl)
            {
                return csl.Id;
            }
            else
            {
                return new EventId(0, string.Empty);
            }
        }
        
        private static string Formatter(LogState? state, Exception? exception)
        {
            var str = state?.ToJson(indent: false, ignore_null: false) ?? string.Empty;
            if (!string.IsNullOrEmpty(str) && !(exception is null))
            {
                str += " ";
            }
            str += exception?.ToString() ?? string.Empty;
            return str;
        }
    }
}
