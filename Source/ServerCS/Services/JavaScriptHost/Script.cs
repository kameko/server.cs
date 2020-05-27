
namespace ServerCS.Services.JavaScriptHost
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Jint;
    
    public class Script
    {
        private Engine engine;
        private Action<Options> options;
        private IScriptSetup setup;
        private List<IBehavior> behaviors;
        
        public Script(Action<Options> script_options, IScriptSetup script_setup)
        {
            setup     = script_setup;
            options   = script_options;
            engine    = setup.Setup(options);
            behaviors = new List<IBehavior>();
        }
        
        public void Reset(Action<Options>? script_options = null)
        {
            script_options ??= options;
            options = script_options;
            engine  = setup.Reset(script_options);
            foreach (var behavior in behaviors)
            {
                behavior.HookEngine(engine);
            }
        }
        
        public bool AddBehavior(string name, IBehavior behavior)
        {
            if (!behaviors.Exists(x => x.Name == name))
            {
                behaviors.Add(behavior);
                behavior.HookEngine(engine);
                return true;
            }
            return false;
        }
        
        public bool RemoveBehavior(string name)
        {
            var behavior = behaviors.Find(x => x.Name == name);
            return behaviors.Remove(behavior!);
        }
        
        public bool TryGetBehavior(string name, out IBehavior? behavior)
        {
            behavior = behaviors.Find(x => x.Name == name);
            return !(behavior is null);
        }
        
        public bool TryGetBehavior<T>(string name, out T? behavior) where T : class, IBehavior
        {
            var success = TryGetBehavior(name, out var ibehavior);
            if (success && ibehavior is T tbehavior)
            {
                behavior = tbehavior;
                return true;
            }
            behavior = null;
            return false;
        }
        
        public void SetBehaviorOrder(IEnumerable<string> order)
        {
            if (order.Count() != behaviors.Count)
            {
                throw new ArgumentException(
                    $"Number of items in collection ({order.Count()}) does not equal "
                  + $"the number of items in the behaviors collection ({behaviors.Count})."
                );
            }
            
            var new_behaviors = new List<IBehavior>();
            foreach (var name in order)
            {
                var success = TryGetBehavior(name, out var behavior);
                if (success)
                {
                    new_behaviors.Add(behavior!);
                }
                else
                {
                    throw new ArgumentException($"Element \"{name}\" does not exist in stored behaviors.");
                }
            }
            
            behaviors = new_behaviors;
        }
        
        public bool SetBehaviorFirst(string name)
        {
            var success = TryGetBehavior(name, out var behavior);
            if (success)
            {
                behaviors.Remove(behavior!);
                behaviors.Insert(0, behavior!);
                return true;
            }
            return false;
        }
        
        public bool SetBehaviorLast(string name)
        {
            var success = TryGetBehavior(name, out var behavior);
            if (success)
            {
                behaviors.Remove(behavior!);
                behaviors.Add(behavior!);
                return true;
            }
            return false;
        }
    }
}
