
namespace ServerCS.Services.JavaScriptHost.Obsolete
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
    
    // FIXME: this is all a bit janky, probably need to
    // heavily refactor. Basically rewrite all of this
    // to be way more modular, split it into classes and stuff.
    // - Get rid of the Call method, instead have pluggable strongly-
    //   typed classes that translate the calls.
    // - Redo the storage system to either accept anything instead of just
    //   a string, or accept an object instead of a string if not possible.
    
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
        
        private string startup_file_cache = string.Empty;
        private string js_file_cache = string.Empty;
        
        public Dictionary<string, Action> Permissions { get; set; }
        public string ScriptName => js_file.Name;
        
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
            
            Permissions    = new Dictionary<string, Action>();
            
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
            config = js_config;
            Reset();
            ClearLocalStorage();
            SetupLocals();
        }
        
        public void Reset()
        {
            Cancel();
            cts       = new CancellationTokenSource();
            js_engine = SetupEngine(config);
            
            foreach (var (permission, callback) in Permissions)
            {
                callback.Invoke();
            }
            
            if (!string.IsNullOrWhiteSpace(startup_file_cache))
            {
                js_engine.Execute(startup_file_cache);
            }
            
            js_engine.Execute(js_file_cache);
        }
        
        public void ClearLocalStorage()
        {
            local_storage.Clear();
        }
        
        public void Initialize()
        {
            if (startup_file.Exists && startup_file.Extension.ToLower() == ".js")
            {
                var js_startup_source = File.ReadAllText(startup_file.FullName);
                js_engine.Execute(js_startup_source);
                startup_file_cache = js_startup_source;
            }
            
            var js_main_source = File.ReadAllText(js_file.FullName);
            js_engine.Execute(js_main_source);
            js_file_cache = js_main_source;
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
        
        public void GrantLocalStorage()
        {
            Permissions.TryAdd(nameof(GrantLocalStorage), GrantLocalStorage);
            
            js_engine.SetValue("__storage__enabled", true);
            
            js_engine.SetValue("__storage__get", new Func<string, string?>(
                key =>
                {
                    var elm = local_storage.Get(key);
                    if (elm is StringJsStorageElement sjsse)
                    {
                        return sjsse.Value;
                    }
                    return null;
                }
            ));
            js_engine.SetValue("__storage__set", new Action<string, string>(
                (key, value) =>
                {
                    local_storage.Set(key, new StringJsStorageElement(key, value));
                }
            ));
        }
        
        public void GrantGlobalStorage()
        {
            Permissions.TryAdd(nameof(GrantGlobalStorage), GrantGlobalStorage);
            
            js_engine.SetValue("__storage__global__enabled", true);
            
            js_engine.SetValue("__storage__global__get", new Func<string, string?>(
                key =>
                {
                    var elm = global_storage.Get(key);
                    if (elm is StringJsStorageElement sjsse)
                    {
                        return sjsse.Value;
                    }
                    return null;
                }
            ));
            js_engine.SetValue("__storage__global__set", new Action<string, string>(
                (key, value) =>
                {
                    global_storage.Set(key, new StringJsStorageElement(key, value));
                }
            ));
        }
        
        public void GrantLogging()
        {
            Permissions.TryAdd(nameof(GrantLogging), GrantLogging);
            
            js_engine.SetValue("__log__enabled", true);
            
            js_engine.SetValue("__log__information" , new Action<string>(s => log.Information(s)));
            js_engine.SetValue("__log__warning"     , new Action<string>(s => log.Warning(s)));
            js_engine.SetValue("__log__error"       , new Action<string>(s => log.Error(s)));
            js_engine.SetValue("__log__critical"    , new Action<string>(s => log.Critical(s)));
            js_engine.SetValue("__log__debug"       , new Action<string>(s => log.Debug(s)));
            js_engine.SetValue("__log__trace"       , new Action<string>(s => log.Trace(s)));
        }
        
        public void GrantExistentialDread()
        {
            Permissions.TryAdd(nameof(GrantExistentialDread), GrantExistentialDread);
            
            js_engine.SetValue("__lifetime__enabled", true);
            
            js_engine.SetValue("__lifetime__shutdown", new Action(
                () =>
                {
                    log.Information($"System shutdown requested from JavaScript script at \"{js_file.FullName}\".");
                    Task.Run(async () =>
                    {
                        await Task.Delay(500);
                        LifetimeEventsHostedService.StopApplication();
                    });
                }
            ));
        }
        
        public void GrantImporting(bool local_only = false)
        {
            Permissions.TryAdd(nameof(GrantImporting), () => GrantImporting(local_only));
            
            js_engine.SetValue("__environment__has_importing", true);
            
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
            GrantLocalStorage();
            GrantGlobalStorage();
            GrantLogging();
            GrantExistentialDread();
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
            
            engine.SetValue("__log__enabled", false);
            engine.SetValue("__lifetime__enabled", false);
            engine.SetValue("__environment__has_importing", false);
            
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
