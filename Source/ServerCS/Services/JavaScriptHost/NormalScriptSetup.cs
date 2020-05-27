
namespace ServerCS.Services.JavaScriptHost
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Jint;
    
    public class NormalScriptSetup : IScriptSetup
    {
        public string Name { get; private set; }
        
        private Engine? stored_engine;
        
        public NormalScriptSetup()
        {
            Name = nameof(NormalScriptSetup);
        }
        
        public Engine Setup(Action<Options> options)
        {
            
            var engine = new Engine(options);
            stored_engine = engine;
            
            return engine;
        }
        
        public Engine Reset(Action<Options> options) => Setup(options);
    }
}
