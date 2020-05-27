
namespace ServerCS.Services.JavaScriptHost.Obsolete
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    
    public class JsStorage
    {
        private ConcurrentDictionary<string, JsStorageElement> elements;
        
        public JsStorage()
        {
            elements = new ConcurrentDictionary<string, JsStorageElement>();
        }
        
        public JsStorageElement? this[string key]
        {
            get => Get(key);
            set => Set(key, value!);
        }
        
        public JsStorageElement? Get(string name)
        {
            elements.TryGetValue(name, out var element);
            return element;
        }
        
        public JsStorageElement<T>? Get<T>(string name)
        {
            return Get(name) as JsStorageElement<T>;
        }
        
        public void Set(string name, JsStorageElement element)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }
            
            elements.AddOrUpdate(name, element, (_, _) => element);
        }
        
        public void Set(JsStorageElement element) => Set(element.Name, element);
        
        public bool HasKey(string key)
        {
            return elements.ContainsKey(key);
        }
        
        public void Clear()
        {
            elements.Clear();
        }
    }
}
