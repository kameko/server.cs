
namespace ServerCS.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Personalities;
    
    public class PersonalityHostedService : IHostedService
    {
        private IConfiguration config;
        private ILogger log;
        
        public PersonalityHostedService(IConfiguration configuration, ILogger<PersonalityHostedService> logger)
        {
            config        = configuration;
            log           = logger;
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
