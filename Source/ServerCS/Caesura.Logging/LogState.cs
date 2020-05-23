
namespace Caesura.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.Json;
    
    public class LogState
    {
        public string Message { get; set; }
        public List<StateElement> Values { get; set; }
        
        public LogState(string message, List<StateElement> values)
        {
            Message = message;
            Values  = values;
        }
        
        public static LogState Create(string message, object[] args)
        {
            var elms  = new List<StateElement>(args.Length);
            var count = 0;
            foreach (var item in args)
            {
                var elm = new StateElement(count, item);
                elms.Add(elm);
                count++;
            }
            var state = new LogState(message, elms);
            return state;
        }
        
        public string ToJson(bool indent, bool ignore_null)
        {
            return ToJson(indent, ignore_null, false);
        }
        
        public string ToJson(bool indent, bool ignore_null, bool json_tag)
        {
            var opt = new JsonSerializerOptions()
            {
                WriteIndented    = indent,
                IgnoreNullValues = ignore_null,
            };
            
            var builder   = new StringBuilder();
            var positions = new Dictionary<int, string>();
            
            // I have no idea how this works but I'm impressed
            // I managed to make it work.
            // And no, I'm not touching regex.
            
            var message       = Message;
            var last_char     = '\0';
            var pos_mode      = false;
            var ignore_double = false;
            var count         = 0;
            foreach (var c in message)
            {
                if (c == '{')
                {
                    if (last_char != '{')
                    {
                        pos_mode = true;
                    }
                    else
                    {
                        ignore_double = true;
                    }
                }
                else if (c == '}')
                {
                    if (last_char != '}')
                    {
                        pos_mode = false;
                        var str = builder.ToString();
                        if (!string.IsNullOrEmpty(str) && !positions.ContainsValue(str))
                        {
                            positions.Add(count, str);
                            count++;
                        }
                        builder.Clear();
                    }
                    else
                    {
                        ignore_double = false;
                    }
                }
                else if (pos_mode && !ignore_double)
                {
                    builder.Append(c);
                }
                
                last_char = c;
            }
            builder.Clear();
            
            builder.Append(message);
            foreach (var (position, str_val) in positions)
            {
                var item = Values.Find(x => x.Position == position);
                if (!(item is null))
                {
                    var json = JsonSerializer.Serialize<object>(item.Value, opt);
                    if (json_tag)
                    {
                        // yes this is a horrible hack for the
                        // SolaceConsoleLoggerFormatter class.
                        json = $"<JSON><$JSON>{json}</JSON>";
                    }
                    builder.Replace("{" + str_val + "}", json);
                }
            }
            
            builder.Replace("}}", "}");
            builder.Replace("{{", "{");
            
            return builder.ToString();
        }
        
        public override string ToString()
        {
            return ToJson(indent: true, ignore_null: true);
        }
        
        public class StateElement
        {
            public int Position { get; set; }
            public object Value { get; set; }
            
            public StateElement(int pos, object val)
            {
                Position = pos;
                Value    = val;
            }
        }
    }
}
