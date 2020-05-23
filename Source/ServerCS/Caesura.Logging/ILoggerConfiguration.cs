
namespace Caesura.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    
    public interface ILoggerConfiguration
    {
        LogLevel LogLevel { get; set; }
        int EventId { get; set; }
    }
}
