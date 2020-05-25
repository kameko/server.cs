
namespace ServerCS.ConfigurationModels
{
    using System;
    using System.Collections.Generic;
    
    public class JavaScriptModel
    {
        public string Directory { get; set; } = string.Empty;
        public string StartupScriptPath { get; set; } = string.Empty;
        public int Timeout { get; set; }
        public int LimitMemory { get; set; }
        public int LimitRecursion { get; set; }
        public bool AllowDebuggerStatement { get; set; }
        public bool DiscardGlobal { get; set; }
        public string AssemblyIncludes { get; set; } = string.Empty;
        public IDictionary<string, JavaScriptValueModel> Values { get; set; } = new Dictionary<string, JavaScriptValueModel>();
    }
    
    public class JavaScriptValueModel
    {
        public string VariableName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
