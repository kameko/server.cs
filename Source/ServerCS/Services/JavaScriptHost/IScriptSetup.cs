
namespace ServerCS.Services.JavaScriptHost
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Jint;
    
    public interface IScriptSetup : IScriptComponent
    {
        Engine Setup(Action<Options> options);
        Engine Reset(Action<Options> options);
    }
}
