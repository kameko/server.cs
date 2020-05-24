
namespace ServerCS.DiscordHandler.Commands
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    
    public class CommandRegistry
    {
        private ConcurrentDictionary<string, string> registry;
        
        public CommandRegistry()
        {
            registry = new ConcurrentDictionary<string, string>();
        }
        
        
    }
}
