
namespace ServerCS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Standard.Logging;
    using ConfigurationModels;
    using Services;
    using DiscordHandler;
    
    public class Runtime
    {
        private IConfiguration config;
        private ILogger log;
        private CancellationTokenSource cts;
        private IDiscordClient discord;
        
        public Runtime(IConfiguration configuration, ILogger<Runtime> logger, IDiscordClient discord_client)
        {
            config  = configuration;
            log     = logger;
            discord = discord_client;
            cts     = new CancellationTokenSource();
            
            LifetimeEventsHostedService.OnStopping += () => cts.Cancel();
            
            log.InstanceAbreaction();
        }
        
        public void Start() => Task.Run(StartAsync);
        
        public Task StartAsync()
        {
            // TODO: read config to get path to .token file, then start discord.
            return Task.CompletedTask;
        }
    }
}
