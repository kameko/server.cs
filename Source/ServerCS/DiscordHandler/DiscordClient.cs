
namespace ServerCS.DiscordHandler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Caesura.Logging;
    using Discord;
    using Discord.WebSocket;
    
    public class DiscordClient : IDiscordClient
    {
        private IConfiguration config;
        private ILogger log;
        private DiscordSocketClient client;
        
        public DiscordClient(IConfiguration configuration, ILogger<DiscordClient> logger)
        {
            config = configuration;
            log    = logger;
            
            var dsc = new DiscordSocketConfig()
            {
                ExclusiveBulkDelete = true,
            };
            
            client = new DiscordSocketClient(dsc);
            
            client.Log += OnLog;
            
            log.InstanceAbreaction();
        }
        
        public async Task Start(string token)
        {
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
        }
        
        public async Task Stop()
        {
            await client.LogoutAsync();
            await client.StopAsync();
        }
        
        
        
        private Task OnLog(LogMessage msg)
        {
            var level = msg.Severity switch
            {
                LogSeverity.Info     => LogLevel.Information,
                LogSeverity.Warning  => LogLevel.Warning,
                LogSeverity.Error    => LogLevel.Error,
                LogSeverity.Critical => LogLevel.Critical,
                LogSeverity.Debug    => LogLevel.Debug,
                LogSeverity.Verbose  => LogLevel.Trace,
                _ => LogLevel.Information
            };
            
            // TODO: check out msg.Source
            return Task.Run(() =>
                log.Raw(level, msg.Exception, msg.Message)
            );
        }
    }
}
