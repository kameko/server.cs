
namespace ServerCS.Services.JavaScriptHost
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.IO;
    using System.Globalization;
    using System.Threading.Tasks;
    using Jint;
    using ConfigurationModels;
    
    public class JsEngineContainer
    {
        public FileInfo FileSource { get; set; }
        public Engine JsEngine { get; set; }
        
        public JsEngineContainer(FileInfo file, JavaScriptModel js_config)
        {
            FileSource = file;
            JsEngine   = new Engine(options =>
            {
                // https://github.com/sebastienros/jint/blob/dev/Jint/Options.cs 
                
                /*
                Hardcode:
                public Options AddObjectConverter(IObjectConverter objectConverter);
                public Options Constraint(IConstraint constraint);
                public Options CatchClrExceptions(Predicate<Exception> handler);
                */
                
                // CONSIDER: public Options SetWrapObjectHandler(Func<Engine, object, ObjectInstance> wrapObjectHandler);
                // CONSIDER: public Options SetReferencesResolver(IReferenceResolver resolver);
                
                options.DebugMode(true);
                options.RegexTimeoutInterval(TimeSpan.FromSeconds(10));
                options.Culture(CultureInfo.CurrentCulture);
                
                if (js_config.LimitMemory > 0)
                {
                    options.LimitMemory(js_config.LimitMemory);
                }
                if (js_config.LimitRecursion > 0)
                {
                    options.LimitRecursion(js_config.LimitRecursion);
                }
                
                options.AllowDebuggerStatement(js_config.AllowDebuggerStatement);
                options.DiscardGlobal(js_config.DiscardGlobal);
                
                // TODO: assembly inclusion.
                // options.AllowClr()
                
                
            });
            
            
        }
        
        private void SetValues()
        {
            // TODO: get values out of a local config.
            /*
            switch (model.Type.ToLower())
            {
                case "string":
                    JsEngine.SetValue()
                    break;
            }
            */
        }
    }
}
