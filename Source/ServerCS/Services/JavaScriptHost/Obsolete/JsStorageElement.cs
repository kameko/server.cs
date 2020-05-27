
namespace ServerCS.Services.JavaScriptHost.Obsolete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Numerics;
    
    public class JsStorageElement
    {
        public string Name { get; set; }
        
        public JsStorageElement(string name)
        {
            Name = name;
        }
        
        public static JsStorageElement FromTypeString(string name, string type, string value)
        {
            switch (type.ToLower())
            {
                case "boolean":
                    var bool_success = bool.TryParse(value, out var bool_value);
                    if (bool_success) return new BooleanJsStorageElement(name, bool_value);
                    else return new InvalidJsStorageElement(name);
                case "number":
                    var number_success = double.TryParse(value, out var number_value);
                    if (number_success) return new NumberJsStorageElement(name, number_value);
                    else return new InvalidJsStorageElement(name);
                case "bigint":
                    var bigint_success = BigInteger.TryParse(value, out var bigint_value);
                    if (bigint_success) return new BigIntJsStorageElement(name, bigint_value);
                    else return new InvalidJsStorageElement(name);
                case "string":
                    return new StringJsStorageElement(name, value);
                case "json":
                    return new JsonJsStorageElement(name, value);
                case "symbol":
                    return new SymbolJsStorageElement(name, value);
                case "null":
                    return new NullJsStorageElement(name);
                case "undefined":
                    return new UndefinedJsStorageElement(name);
                default:
                    return new InvalidJsStorageElement(name);
            }
        }
        
        public static bool HasValue(JsStorageElement element)
        {
            return !(
                element is InvalidJsStorageElement   ||
                element is UndefinedJsStorageElement ||
                element is NullJsStorageElement      ||
                element is null
            );
        }
    }
    
    public class InvalidJsStorageElement : JsStorageElement
    {
        public InvalidJsStorageElement(string name) : base(name)
        {
            
        }
    }
    
    public abstract class JsStorageElement<T> : JsStorageElement
    {
        public T Value { get; set; }
        
        public JsStorageElement(string name, T value) : base(name)
        {
            Value = value;
        }
    }
    
    public class NullJsStorageElement : JsStorageElement
    {
        public NullJsStorageElement(string name) : base(name)
        {
            
        }
    }
    
    public class UndefinedJsStorageElement : JsStorageElement
    {
        public UndefinedJsStorageElement(string name) : base(name)
        {
            
        }
    }
    
    public class StringJsStorageElement : JsStorageElement<string>
    {
        public StringJsStorageElement(string name, string value) : base(name, value)
        {
            
        }
    }
    
    public class SymbolJsStorageElement : JsStorageElement<string>
    {
        public SymbolJsStorageElement(string name, string value) : base(name, value)
        {
            
        }
    }
    
    public class BooleanJsStorageElement : JsStorageElement<bool>
    {
        public BooleanJsStorageElement(string name, bool value) : base(name, value)
        {
            
        }
    }
    
    public class NumberJsStorageElement : JsStorageElement<double>
    {
        public NumberJsStorageElement(string name, double value) : base(name, value)
        {
            
        }
    }
    
    public class BigIntJsStorageElement : JsStorageElement<BigInteger>
    {
        public BigIntJsStorageElement(string name, BigInteger value) : base(name, value)
        {
            
        }
    }
    
    public class JsonJsStorageElement : JsStorageElement<string>
    {
        public JsonJsStorageElement(string name, string value) : base(name, value)
        {
            
        }
    }
}
