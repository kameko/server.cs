
namespace ServerCS.Services.JavaScriptHost
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Jint;
    using Jint.Native;
    
    public interface IBehavior : IScriptComponent
    {
        void HookEngine(Engine engine);
        JsValue JsCall(string func_name, params JsValue[] arguments);
    }
    
    public interface IBehaviorAction : IBehavior
    {
        void Call();
    }
    
    public interface IBehaviorAction<T0> : IBehavior
    {
        void Call(T0 arg0);
    }
    
    public interface IBehaviorAction<T0, T1> : IBehavior
    {
        void Call(T0 arg0, T1 arg1);
    }
    
    public interface IBehaviorAction<T0, T1, T2> : IBehavior
    {
        void Call(T0 arg0, T1 arg1, T2 arg2);
    }
    
    public interface IBehaviorAction<T0, T1, T2, T3> : IBehavior
    {
        void Call(T0 arg0, T1 arg1, T2 arg2, T3 arg3);
    }
    
    public interface IBehaviorAction<T0, T1, T2, T3, T4> : IBehavior
    {
        void Call(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    }
    
    public interface IBehaviorFunc<T0, R> : IBehavior
    {
        R Call(T0 arg0);
    }
    
    public interface IBehaviorFunc<T0, T1, R> : IBehavior
    {
        R Call(T0 arg0, T1 arg1);
    }
    
    public interface IBehaviorFunc<T0, T1, T2, R> : IBehavior
    {
        R Call(T0 arg0, T1 arg1, T2 arg2);
    }
    
    public interface IBehaviorFunc<T0, T1, T2, T3, R> : IBehavior
    {
        R Call(T0 arg0, T1 arg1, T2 arg2, T3 arg3);
    }
    
    public interface IBehaviorFunc<T0, T1, T2, T3, T4, R> : IBehavior
    {
        R Call(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    }
}
