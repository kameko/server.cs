
namespace Caesura.Logging
{
    using System;
    using System.Collections.Generic;
    
    public static class Themes
    {
        static Themes()
        {
            Gray = new ConsoleTheme()
            {
                Name                       = "Gray",
                Credit                     = "Caesura",
            };
            
            Native = new ConsoleTheme()
            {
                Name                       = "Native",
                Credit                     = "Caesura",
                InfoColor                  = ConsoleColor.DarkCyan,
                WarnColor                  = ConsoleColor.Yellow,
                ErrorColor                 = ConsoleColor.Red,
                CriticalColor              = ConsoleColor.DarkRed,
                DebugColor                 = ConsoleColor.Gray,
                TraceColor                 = ConsoleColor.DarkGray,
                BracketColor               = ConsoleColor.DarkGray,
                TimeStampColor             = ConsoleColor.DarkGray,
                NameColor                  = ConsoleColor.DarkGray,
                MessageColor               = ConsoleColor.Gray,
                JsonColor                  = ConsoleColor.DarkCyan,
                ExceptionWarningColor      = ConsoleColor.DarkRed,
                ExceptionMetaColor         = ConsoleColor.DarkRed,
                ExceptionNameColor         = ConsoleColor.DarkRed,
                ExceptionMessageColor      = ConsoleColor.Red,
                ExceptionStackTraceColor   = ConsoleColor.Red,
            };
            
            BlueAsMySoul = new ConsoleTheme()
            {
                Name                       = "Blue as my Soul",
                Credit                     = "Caesura",
                InfoColor                  = ConsoleColor.Blue,
                WarnColor                  = ConsoleColor.Yellow,
                ErrorColor                 = ConsoleColor.Red,
                CriticalColor              = ConsoleColor.DarkRed,
                DebugColor                 = ConsoleColor.Gray,
                TraceColor                 = ConsoleColor.DarkGray,
                BracketColor               = ConsoleColor.DarkGray,
                TimeStampColor             = ConsoleColor.Blue,
                NameColor                  = ConsoleColor.Blue,
                MessageColor               = ConsoleColor.Blue,
                JsonColor                  = ConsoleColor.Blue,
                ExceptionWarningColor      = ConsoleColor.DarkRed,
                ExceptionMetaColor         = ConsoleColor.DarkRed,
                ExceptionNameColor         = ConsoleColor.DarkRed,
                ExceptionMessageColor      = ConsoleColor.Red,
                ExceptionStackTraceColor   = ConsoleColor.Red,
            };
            
            Collection = new ThemesCollection(new Dictionary<string, IConsoleTheme>()
                {
                    { Gray.Name         , Gray         },
                    { Native.Name       , Native       },
                    { BlueAsMySoul.Name , BlueAsMySoul },
                }
            );
        }
        
        public static IConsoleTheme Gray { get; private set; }
        public static IConsoleTheme Native { get; private set; }
        public static IConsoleTheme BlueAsMySoul { get; private set; }
        
        public static ThemesCollection Collection { get; private set; }
        
        public class ThemesCollection
        {
            private Dictionary<string, IConsoleTheme> themes_collection;
            public Dictionary<string, IConsoleTheme> Themes => themes_collection;
            public int Count => themes_collection.Count;
            
            internal ThemesCollection(Dictionary<string, IConsoleTheme> themes)
            {
                themes_collection = themes;
            }
            
            public IConsoleTheme this[string name]
            {
                get => Get(name);
                set => Add(name, value);
            }
            
            public IConsoleTheme Get(string name)
            {
                if (!themes_collection.ContainsKey(name))
                {
                    return new ConsoleTheme() { Name = "Unknown Theme" };
                }
                return themes_collection[name];
            }
            
            public void Add(string name, IConsoleTheme theme)
            {
                if (!themes_collection.ContainsKey(name))
                {
                    themes_collection.Add(name, theme);
                }
            }
        }
    }
}
