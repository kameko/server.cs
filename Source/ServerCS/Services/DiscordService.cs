
namespace ServerCS.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Hosting;
    using Standard.Logging;
    using ConfigurationModels;
    using DiscordHandler;
    
    public class DiscordService : IHostedService
    {
        private IConfiguration config;
        private DiscordModel discord_config;
        private ILogger log;
        private IDiscordClient discord;
        
        public DiscordService(IConfiguration configuration, ILogger<DiscordService> logger, IDiscordClient discord_client)
        {
            config         = configuration;
            discord_config = DiscordModel.FromConfiguration(configuration);
            log            = logger;
            discord        = discord_client;
        }
        
        public async Task StartAsync(CancellationToken token)
        {
            // TODO: a way to both log in invisible as well as just not logging in at all.
            var discord_token = await File.ReadAllTextAsync(discord.DiscordConfiguration.TokenFilePath, token);
            discord_token = discord_token.Trim();
            await discord.Start(discord_token);
        }
        
        public async Task StopAsync(CancellationToken token)
        {
            await discord.Stop();
        }
    }
}
