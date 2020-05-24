
namespace ServerCS.DiscordHandler.Commands
{
    using System;
    using System.Collections.Concurrent;
    
    public class CommandRegistry
    {
        private ConcurrentDictionary<string, string> registry;
        
        public CommandRegistry()
        {
            registry = new ConcurrentDictionary<string, string>();
        }
        
        public string? this[string key]
        {
            get => Get(key);
            set => Set(key, value!);
        }
        
        public string? Get(string key)
        {
            var success = registry.TryGetValue(key, out var value);
            if (success)
            {
                return value;
            }
            return null;
        }
        
        public void Set(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
            {
                return;
            }
            
            registry.AddOrUpdate(key, value, (_, _) => value);
        }
        
        public bool HasKey(string key)
        {
            return registry.ContainsKey(key);
        }
    }
}
