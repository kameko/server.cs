
namespace ServerCS.Services.JavaScriptHost
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.IO;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using Jint;
    using Jint.Native;
    using ConfigurationModels;
    
    public class JsEngineContainer : IDisposable
    {
        private JavaScriptModel config;
        private FileInfo startup_file;
        private FileInfo js_file;
        private JsStorage global_storage;
        private JsStorage local_storage;
        private CancellationTokenSource cts;
        private Engine js_engine;
        
        public JsEngineContainer(JavaScriptModel js_config, FileInfo startup_file_source, FileInfo js_file_source, JsStorage global)
        {
            config         = js_config;
            startup_file   = startup_file_source;
            js_file        = js_file_source;
            global_storage = global;
            local_storage  = new JsStorage();
            cts            = new CancellationTokenSource();
            js_engine      = SetupEngine(js_config);
            
            SetupLocals();
        }
        
        public static bool FromPaths(
            JavaScriptModel js_config,
            JsStorage global,
            string startup_source_path,
            string main_source_path,
            out JsEngineContainer? container
        )
        {
            var startup_fi = new FileInfo(startup_source_path);
            var main_fi    = new FileInfo(main_source_path);
            var success    = validate(startup_fi) && validate(main_fi);
            if (success)
            {
                container = new JsEngineContainer(js_config, startup_fi, main_fi, global);
                return true;
            }
            else
            {
                container = null;
                return false;
            }
            
            bool validate(FileInfo fi) => fi.Exists && fi.Extension.ToLower() == ".js";
        }
        
        public void Reconfigure(JavaScriptModel js_config)
        {
            Cancel();
            cts       = new CancellationTokenSource();
            config    = js_config;
            js_engine = SetupEngine(config);
            local_storage.Clear();
            SetupLocals();
        }
        
        public void Initialize()
        {
            var js_startup_source = File.ReadAllText(startup_file.FullName);
            var js_main_source    = File.ReadAllText(js_file.FullName);
            
            js_engine.Execute(js_startup_source);
            js_engine.Execute(js_main_source);
        }
        
        public JsValue Call(string func_name, params object[]? args)
        {
            var func = js_engine.GetValue(func_name);
            
            if (!func.IsObject())
            {
                return JsValue.FromObject(js_engine, null);
            }
            
            if (!(args is null) && args.Length > 0)
            {
                var js_args = new JsValue[args.Length];
                for (int i = 0; i > args.Length; i++)
                {
                    var arg_i = args[i];
                    if (arg_i is JsValue jsv)
                    {
                        js_args[i] = jsv;
                    }
                    else
                    {
                        js_args[i] = JsValue.FromObject(js_engine, arg_i);
                    }
                }
                
                return func.Invoke(js_args);
            }
            else
            {
                return func.Invoke();
            }
        }
        
        public void Cancel()
        {
            cts.Cancel();
        }
        
        public void Dispose()
        {
            Cancel();
        }
        
        private Engine SetupEngine(JavaScriptModel js_config)
        {
            return new Engine(options =>
            {
                // https://github.com/sebastienros/jint/blob/dev/Jint/Options.cs 
                
                /* TODO:
                Hardcode:
                    AddObjectConverter(IObjectConverter objectConverter);
                    CatchClrExceptions(Predicate<Exception> handler);
                Consider:
                    SetWrapObjectHandler(Func<Engine, object, ObjectInstance> wrapObjectHandler);
                    SetReferencesResolver(IReferenceResolver resolver);
                */
                
                options.Strict(true);
                options.DebugMode(true);
                options.RegexTimeoutInterval(TimeSpan.FromSeconds(10));
                options.Culture(CultureInfo.CurrentCulture);
                options.CancellationToken(cts.Token);
                
                if (js_config.Timeout > 0)
                {
                    options.TimeoutInterval(TimeSpan.FromMilliseconds(js_config.Timeout));
                }
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
                
                var asms = js_config.AssemblyIncludes.Split(';', ',');
                foreach (var asm_name_iter in asms)
                {
                    var asm_name = asm_name_iter.Trim().ToLower();
                    if (asm_name == "system")
                    {
                        options.AllowClr();
                    }
                    else
                    {
                        // TODO:
                    }
                }
            });
        }
        
        private void SetupLocals()
        {
            foreach (var (_, value_model) in config.Values)
            {
                var js_value = JsStorageElement.FromTypeString(value_model.VariableName, value_model.Type, value_model.Value);
                if (JsStorageElement.HasValue(js_value))
                {
                    local_storage.Set(js_value);
                }
            }
        }
        
        private void SetValues()
        {
            
        }
    }
}
