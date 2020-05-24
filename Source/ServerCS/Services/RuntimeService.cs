
namespace ServerCS.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Hosting;
    using Standard.Logging;
    
    public class RuntimeService : IHostedService
    {
        public RuntimeService(IConfiguration configuration, ILogger<RuntimeService> logger)
        {
            
        }
        
        public Task StartAsync(CancellationToken token)
        {
            return Task.CompletedTask;
        }
        
        public Task StopAsync(CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}
