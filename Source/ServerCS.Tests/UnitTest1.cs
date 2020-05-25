
namespace ServerCS.Tests
{
    using System;
    using Xunit;
    using Xunit.Abstractions;
    using Jint;
    using Jint.Native;
    
    public class UnitTest1
    {
        private ITestOutputHelper _output;
        
        public UnitTest1(ITestOutputHelper output)
        {
            _output = output;
            _output.WriteLine(string.Empty);
        }
        
        [Fact]
        public void Test1()
        {
            var engine = new Engine();
            engine.Execute("function add(a, b) { return a + b; }");
            var v1 = engine.GetValue("add");
            var v2 = engine.GetValue("non-existant");
            _output.WriteLine($"{v1.Type}");
            _output.WriteLine($"{v2.Type}");
            var v3 = v1.Invoke(1, 2);
            _output.WriteLine($"{v3.Type}");
            _output.WriteLine($"{v3.AsNumber()}");
            var v4 = JsValue.FromObject(engine, null);
            _output.WriteLine($"{v4.Type}");
            
            var js_obj_raw = new TestObj1() { Id = 10, Content = "Hello!" };
            var v5 = JsValue.FromObject(engine, js_obj_raw);
            _output.WriteLine($"{v5.Type}");
            var v6 = v5.AsObject().Get(JsValue.FromObject(engine, "content"));
            _output.WriteLine($"{v6.Type}");
            _output.WriteLine($"{v6.AsString()}");
        }
        
        public class TestObj1
        {
            public ulong Id { get; set; }
            public string Content { get; set; } = string.Empty;
        }
    }
}
