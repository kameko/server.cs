
namespace Caesura.Logging
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
    
    public class LogElement : IHasId<ulong>
    {
        public ulong Id                          { get; set; }
        
        public DateTime TimeStamp                { get; set; }
        public LogLevel Level                    { get; set; }
        public int EventId                       { get; set; }
        public string SenderService              { get; set; }
        public string ReceiverService            { get; set; }
        public string Message                    { get; set; }
        public IEnumerable<ItemElement> Elements { get; set; }
        public ExceptionElement Exception        { get; set; }
        public ResourceSnapshot ResourceUse      { get; set; }
        
        public LogElement()
        {
            SenderService   = string.Empty;
            ReceiverService = string.Empty;
            Message         = string.Empty;
            Elements        = new List<ItemElement>();
            Exception       = new ExceptionElement();
            ResourceUse     = new ResourceSnapshot();
        }
        
        public override string ToString()
        {
            return 
                $"[{Level}][{SenderService}({Id})]: {Message}{(string.IsNullOrEmpty(Message) ? string.Empty : " ")} " + 
                $"{(Exception is null ? string.Empty : Environment.NewLine)}{Exception}";
        }
        
        public class ItemElement : IHasId<ulong>
        {
            public ulong Id     { get; set; }
            public int Position { get; set; }
            public string Value { get; set; }
            
            public ItemElement()
            {
                Value = string.Empty;
            }
        }
        
        public class ResourceSnapshot : IHasId<ulong>
        {
            public ulong Id              { get; set; }
            public int RamMbUse          { get; set; }
            public int CpuPercentUse     { get; set; }
            public int NetworkPercentUse { get; set; } // TODO: remove, doesn't seem like we can get this
            // TODO: GC stuff
            
            // TODO: Generate this with:
            // using System.Diagnostics.Process;
            // var me = Process.GetCurrentProcess();
            // me.WorkingSet64 (bytes, convert to MiB)
            // me.TotalProcessorTime.TotalSeconds
            // etc.
            // https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.process?view=netcore-3.1 
            
            public ResourceSnapshot()
            {
                
            }
        }
        
        public class ExceptionElement : IHasId<ulong>
        {
            public ulong Id       { get; set; }
            public string Name    { get; set; } = string.Empty;
            public string Message { get; set; } = string.Empty;
            public string Stack   { get; set; } = string.Empty;
            public bool HasValue  { get; set; } = false;
            
            public ExceptionElement()
            {
                
            }
            
            public ExceptionElement(Exception? ex)
            {
                if (!(ex is null))
                {
                    HasValue = true;
                    Name     = ex.GetType().FullName ?? string.Empty;
                    Message  = ex.Message;
                    Stack    = ex.StackTrace ?? string.Empty;
                }
            }
        }
    }
}
