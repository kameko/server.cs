
namespace ServerCS.Services.JavaScriptHost
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.IO;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Standard.Logging;
    using Jint;
    using Jint.Runtime.Interop;
    using Jint.Native;
    using ConfigurationModels;
    
    public class JsEngineContainer : IDisposable
    {
        private ILogger log;
        private JavaScriptModel config;
        private FileInfo startup_file;
        private FileInfo js_file;
        private JsStorage global_storage;
        private JsStorage local_storage;
        private CancellationTokenSource cts;
        private Engine js_engine;
        
        public JsEngineContainer(ILogger logger, JavaScriptModel js_config, FileInfo startup_file_source, FileInfo js_file_source, JsStorage global)
        {
            log            = logger;
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
            ILogger logger,
            JavaScriptModel js_config,
            JsStorage global,
            string startup_source_path,
            string main_source_path,
            out JsEngineContainer? container
        )
        {
            var startup_fi = new FileInfo(startup_source_path);
            var main_fi    = new FileInfo(main_source_path);
            var success    = main_fi.Exists && main_fi.Extension.ToLower() == ".js";
            if (success)
            {
                container = new JsEngineContainer(logger, js_config, startup_fi, main_fi, global);
                return true;
            }
            else
            {
                container = null;
                return false;
            }
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
            if (startup_file.Exists && startup_file.Extension.ToLower() == ".js")
            {
                var js_startup_source = File.ReadAllText(startup_file.FullName);
                js_engine.Execute(js_startup_source);
            }
            
            var js_main_source = File.ReadAllText(js_file.FullName);
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
                for (int i = 0; i < args.Length; i++)
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
        
        public void GrantLogging()
        {
            js_engine.SetValue("__log__information" , new Action<string>(s => log.Information(s)));
            js_engine.SetValue("__log__warning"     , new Action<string>(s => log.Warning(s)));
            js_engine.SetValue("__log__error"       , new Action<string>(s => log.Error(s)));
            js_engine.SetValue("__log__critical"    , new Action<string>(s => log.Critical(s)));
            js_engine.SetValue("__log__debug"       , new Action<string>(s => log.Debug(s)));
            js_engine.SetValue("__log__trace"       , new Action<string>(s => log.Trace(s)));
        }
        
        public void GrantSuicidalTendencies()
        {
            js_engine.SetValue("__lifetime__shutdown", new Action(
                () =>
                {
                    log.Information($"System shutdown requested from JavaScript script at \"{js_file.FullName}\".");
                    LifetimeEventsHostedService.StopApplication();
                }
            ));
        }
        
        public void GrantImporting(bool local_only = false)
        {
            // TODO: test if this works
            js_engine.SetValue("__environment__import", new Action<string>(
                s =>
                {
                    var fi = new FileInfo(s);
                    
                    if (!fi.Exists)
                    {
                        return;
                    }
                    
                    if (fi.Extension.ToLower() != ".js")
                    {
                        return;
                    }
                    
                    if (local_only)
                    {
                        var org_di = new DirectoryInfo(js_file.FullName);
                        var new_di = new DirectoryInfo(fi.FullName);
                        if (!new_di.FullName.ToLower().StartsWith(org_di.FullName.ToLower()))
                        {
                            return;
                        }
                    }
                    
                    var script = File.ReadAllText(fi.FullName);
                    js_engine.Execute(script);
                }
            ));
        }
        
        public void GrantAll()
        {
            GrantLogging();
            GrantSuicidalTendencies();
            GrantImporting();
        }
        
        public void GrantType<T>(string name)
        {
            js_engine.SetValue(name, TypeReference.CreateTypeReference(js_engine, typeof(T)));
        }
        
        public void GrantCallback(string name, Delegate callback)
        {
            js_engine.SetValue(name, callback);
        }
        
        private Engine SetupEngine(JavaScriptModel js_config)
        {
            var engine = new Engine(options =>
            {
                // https://github.com/sebastienros/jint/blob/dev/Jint/Options.cs 
                
                /* TODO:
                Hardcode:
                    AddObjectConverter(IObjectConverter objectConverter);
                Consider:
                    SetWrapObjectHandler(Func<Engine, object, ObjectInstance> wrapObjectHandler);
                    SetReferencesResolver(IReferenceResolver resolver);
                */
                
                options.CatchClrExceptions(e =>
                {
                    // TODO:
                    log.Warning(e, $"JavaScript exception encountered in \"{js_file.FullName}\".");
                    return true;
                });
                
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
            
            return engine;
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
