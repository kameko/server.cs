
namespace Caesura.Logging
{
    using System;
    
    public interface IConsoleTheme
    {
        string Name                           { get; set; }
        string Credit                         { get; set; }
        Version IntendedThemeSystemVersion    { get; set; }
        ConsoleColor InfoColor                { get; set; }
        ConsoleColor WarnColor                { get; set; }
        ConsoleColor ErrorColor               { get; set; }
        ConsoleColor CriticalColor            { get; set; }
        ConsoleColor DebugColor               { get; set; }
        ConsoleColor TraceColor               { get; set; }
        ConsoleColor BracketColor             { get; set; }
        ConsoleColor TimeStampColor           { get; set; }
        ConsoleColor NameColor                { get; set; }
        ConsoleColor MessageColor             { get; set; }
        ConsoleColor JsonColor                { get; set; }
        ConsoleColor ExceptionWarningColor    { get; set; }
        ConsoleColor ExceptionMetaColor       { get; set; }
        ConsoleColor ExceptionNameColor       { get; set; }
        ConsoleColor ExceptionMessageColor    { get; set; }
        ConsoleColor ExceptionStackTraceColor { get; set; }
    }
    
    public class ConsoleTheme : IConsoleTheme
    {
        public string Name                           { get; set; } = "No Name";
        public string Credit                         { get; set; } = "Uncredited";
        public Version IntendedThemeSystemVersion    { get; set; } = new Version(1, 0, 0, 0);
        public ConsoleColor InfoColor                { get; set; } = ConsoleColor.Gray;
        public ConsoleColor WarnColor                { get; set; } = ConsoleColor.Gray;
        public ConsoleColor ErrorColor               { get; set; } = ConsoleColor.Gray;
        public ConsoleColor CriticalColor            { get; set; } = ConsoleColor.Gray;
        public ConsoleColor DebugColor               { get; set; } = ConsoleColor.Gray;
        public ConsoleColor TraceColor               { get; set; } = ConsoleColor.Gray;
        public ConsoleColor BracketColor             { get; set; } = ConsoleColor.Gray;
        public ConsoleColor TimeStampColor           { get; set; } = ConsoleColor.Gray;
        public ConsoleColor NameColor                { get; set; } = ConsoleColor.Gray;
        public ConsoleColor MessageColor             { get; set; } = ConsoleColor.Gray;
        public ConsoleColor JsonColor                { get; set; } = ConsoleColor.Gray;
        public ConsoleColor ExceptionWarningColor    { get; set; } = ConsoleColor.Gray;
        public ConsoleColor ExceptionMetaColor       { get; set; } = ConsoleColor.Gray;
        public ConsoleColor ExceptionNameColor       { get; set; } = ConsoleColor.Gray;
        public ConsoleColor ExceptionMessageColor    { get; set; } = ConsoleColor.Gray;
        public ConsoleColor ExceptionStackTraceColor { get; set; } = ConsoleColor.Gray;
        
        public ConsoleTheme()
        {
            
        }
        
        public ConsoleTheme(IConsoleTheme theme)
        {
            Name                        = theme.Name;
            Credit                      = theme.Credit;
            IntendedThemeSystemVersion  = theme.IntendedThemeSystemVersion;
            InfoColor                   = theme.InfoColor;
            WarnColor                   = theme.WarnColor;
            ErrorColor                  = theme.ErrorColor;
            CriticalColor               = theme.CriticalColor;
            DebugColor                  = theme.DebugColor;
            TraceColor                  = theme.TraceColor;
            BracketColor                = theme.BracketColor;
            TimeStampColor              = theme.TimeStampColor;
            NameColor                   = theme.NameColor;
            MessageColor                = theme.MessageColor;
            JsonColor                   = theme.JsonColor;
            ExceptionWarningColor       = theme.ExceptionWarningColor;
            ExceptionMetaColor          = theme.ExceptionMetaColor;
            ExceptionNameColor          = theme.ExceptionNameColor;
            ExceptionMessageColor       = theme.ExceptionMessageColor;
            ExceptionStackTraceColor    = theme.ExceptionStackTraceColor;
        }
    }
}
