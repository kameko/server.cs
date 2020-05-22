
namespace Caesura.Logging
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
    
    // Abandon all hope ye who enter here
    
    public class SolaceConsoleLoggerFormatter : ISolaceConsoleLoggerFormatter
    {
        private readonly ConsoleColor original_foreground_color;
        private readonly ConsoleColor original_background_color;
        private readonly List<string> errors;
        
        public SolaceConsoleLoggerFormatter()
        {
            Console.ResetColor();
            
            original_foreground_color = Console.ForegroundColor;
            original_background_color = Console.BackgroundColor;
            errors                    = new List<string>();
        }
        
        public void PreLog(LogItem item)
        {
            
        }
        
        public void PostLog(LogItem item)
        {
            Console.ForegroundColor = original_foreground_color;
            Console.BackgroundColor = original_background_color;
        }
        
        public string Format(LogItem item)
        {
            var message = item.State?.ToString() ?? string.Empty;
            var newline = true;
            
            var original_foreground = original_foreground_color;
            var original_background = original_background_color;
            
            if (message.Contains("<$NoStamp>"))
            {
                message = message.Replace("<$NoStamp>", string.Empty);
            }
            else
            {
                StampFormatter(item);
            }
            
            if (message.Contains("<$NoNewLine>"))
            {
                message = message.Replace("<$NoNewLine>", string.Empty);
                newline = false;
            }
            
            if (!string.IsNullOrEmpty(message))
            {
                MessageFormatter(item);
            }
            
            if (!(item.Exception is null))
            {
                if (string.IsNullOrEmpty(message))
                {
                    Write(" ");
                }
                else
                {
                    WriteLine();
                }
                
                ExceptionFormatter(item);
            }
            
            ReportInternalErrors(item);
            
            if (newline)
            {
                WriteLine();
            }
            
            return item.ToString();
        }
        
        private void StampFormatter(LogItem item)
        {
            var original_foreground = Console.ForegroundColor;
            var original_background = Console.BackgroundColor;
            
            var level_color = item.Level switch
            {
                LogLevel.Information => item.Configuration.Theme.InfoColor,
                LogLevel.Warning     => item.Configuration.Theme.WarnColor,
                LogLevel.Error       => item.Configuration.Theme.ErrorColor,
                LogLevel.Critical    => item.Configuration.Theme.CriticalColor,
                LogLevel.Debug       => item.Configuration.Theme.DebugColor,
                LogLevel.Trace       => item.Configuration.Theme.TraceColor,
                LogLevel.None        => ConsoleColor.Gray,
                
                _ => ConsoleColor.Gray
            };
            
            Console.ForegroundColor = item.Configuration.Theme.BracketColor;
            Write("[");
            Console.ForegroundColor = level_color;
            Write(item.Level);
            Console.ForegroundColor = item.Configuration.Theme.BracketColor;
            Write("]");
            Console.ForegroundColor = original_foreground;
            
            Console.ForegroundColor = item.Configuration.Theme.BracketColor;
            Write("[");
            Console.ForegroundColor = item.Configuration.Theme.TimeStampColor;
            Write(item.TimeStamp.ToString(item.Configuration.TimeStampFormat));
            Console.ForegroundColor = item.Configuration.Theme.BracketColor;
            Write("]");
            Console.ForegroundColor = original_foreground;
            
            var name = item.Name;
            foreach (var trim in item.Configuration.TrimNames)
            {
                if (name.StartsWith(trim))
                {
                    name = name.Replace(trim, string.Empty);
                    break;
                }
            }
            foreach (var (val, repl) in item.Configuration.ReplaceNames)
            {
                if (name == val)
                {
                    name = repl;
                    break;
                }
            }
            
            if (!string.IsNullOrEmpty(name))
            {
                Console.ForegroundColor = item.Configuration.Theme.BracketColor;
                Write("[");
                Console.ForegroundColor = item.Configuration.Theme.NameColor;
                Write(name);
                if (item.Id != 0)
                {
                    Write("(");
                    Write(item.Id);
                    Write(")");
                }
                Console.ForegroundColor = item.Configuration.Theme.BracketColor;
                Write("]");
                Console.ForegroundColor = original_foreground;
            }
            
            Write(" ");
        }
        
        private void MessageFormatter(LogItem item)
        {
            var original_foreground = Console.ForegroundColor;
            var original_background = Console.BackgroundColor;
            
            if (item.Configuration.StringifyOption == ConsoleLoggerConfiguration.ObjectStringifyOption.CallToString)
            {
                WriteState();
            }
            else if (item.Configuration.StringifyOption == ConsoleLoggerConfiguration.ObjectStringifyOption.SerializeJsonPretty)
            {
                WriteJson(indent: true);
            }
            else if (item.Configuration.StringifyOption == ConsoleLoggerConfiguration.ObjectStringifyOption.SerializeJsonRaw)
            {
                WriteJson(indent: false);
            }
            else
            {
                errors.Add(
                    $"Unrecognized {nameof(ConsoleLoggerConfiguration.ObjectStringifyOption)} "
                  + $"option: {item.Configuration.StringifyOption}."
                );
            }
            
            void WriteState()
            {
                Console.ForegroundColor = item.Configuration.Theme.MessageColor;
                Write(item.State?.ToString() ?? string.Empty);
                Console.ForegroundColor = original_foreground;
            }
            
            void WriteJson(bool indent)
            {
                if (item.State is SolaceLogState sls)
                {
                    JsonFormatter(item, sls, indent);
                }
                else
                {
                    WriteState();
                }
            }
        }
        
        private void JsonFormatter(LogItem item, SolaceLogState state, bool indent)
        {
            var original_foreground = Console.ForegroundColor;
            var original_background = Console.BackgroundColor;
            
            var json = state.ToJson(indent, ignore_null: true, json_tag: true);
            
            var splits = json.Split("<JSON>");
            foreach (var str in splits)
            {
                if (str.StartsWith("<$JSON>"))
                {
                    var xsplits = str.Replace("<$JSON>", string.Empty).Split("</JSON>");
                    Console.ForegroundColor = item.Configuration.Theme.JsonColor;
                    var xjson = xsplits[0];
                    if (xjson.StartsWith("\"") && xjson.EndsWith("\""))
                    {
                        xjson = xjson.TrimStart('"').TrimEnd('"');
                    }
                    Write(xjson);
                    Console.ForegroundColor = original_foreground;
                    if (xsplits.Length > 1)
                    {
                        Console.ForegroundColor = item.Configuration.Theme.MessageColor;
                        Write(xsplits[1]);
                        Console.ForegroundColor = original_foreground;
                    }
                }
                else
                {
                    Console.ForegroundColor = item.Configuration.Theme.MessageColor;
                    Write(str);
                    Console.ForegroundColor = original_foreground;
                }
            }
        }
        
        private void ExceptionFormatter(LogItem item)
        {
            var exception = item.Exception!;
            
            var original_foreground = Console.ForegroundColor;
            var original_background = Console.BackgroundColor;
            
            Console.ForegroundColor = item.Configuration.Theme.ExceptionWarningColor;
            Write("EXCEPTION: ");
            Console.ForegroundColor = item.Configuration.Theme.ExceptionNameColor;
            Write(exception.GetType().FullName ?? "<NO TYPE NAME>");
            WriteLine();
            
            if (!string.IsNullOrEmpty(exception.Message))
            {
                Console.ForegroundColor = item.Configuration.Theme.ExceptionMetaColor;
                Write("MESSAGE: ");
                Console.ForegroundColor = item.Configuration.Theme.ExceptionMessageColor;
                Write(exception.Message);
                WriteLine();
            }
            
            Console.ForegroundColor = item.Configuration.Theme.ExceptionMetaColor;
            Write(" Stack Trace: ");
            WriteLine();
            Console.ForegroundColor = item.Configuration.Theme.ExceptionStackTraceColor;
            Write(exception.StackTrace ?? " <NO STACK TRACE>");
            Console.ForegroundColor = item.Configuration.Theme.ExceptionMetaColor;
            WriteLine();
            Write(" --- End of stack trace ---");
            
            Console.ForegroundColor = original_foreground;
        }
        
        private void ReportInternalErrors(LogItem item)
        {
            if (errors.Count > 0)
            {
                var original_foreground = Console.ForegroundColor;
                var original_background = Console.BackgroundColor;
                
                var e_count = errors.Count;
                
                WriteLine();
                Console.ForegroundColor = item.Configuration.Theme.ExceptionMessageColor;
                Write(
                    $"{nameof(SolaceConsoleLoggerFormatter)} Error: {e_count} "
                  + $"error{(e_count == 1 ? string.Empty : "s")} encountered."
                );
                
                var ee_count = e_count;
                var d_len = string.Empty;
                if (ee_count >= 10 && ee_count < 100)
                {
                    d_len = "D2";
                }
                else if (ee_count >= 100 && ee_count < 1_000)
                {
                    d_len = "D3";
                }
                else if (ee_count >= 1_000 && ee_count < 10_000)
                {
                    d_len = "D4";
                }
                
                var count = 1;
                foreach (var error in errors)
                {
                    WriteLine();
                    Write($" [{count.ToString(d_len)}]: {error}");
                    count++;
                }
                
                errors.Clear();
                
                Console.ForegroundColor = original_foreground;
            }
        }
        
        private void Write(object item)
        {
            var str = item.ToString();
            Write(str!);
        }
        
        private void Write(string str)
        {
            Console.Write(str);
        }
        
        private void WriteLine()
        {
            Console.WriteLine();
        }
    }
}
