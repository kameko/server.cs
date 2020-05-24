
namespace ServerCS.DiscordHandler.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Standard.Logging;
    using ConfigurationModels;
    using Discord;
    using Discord.WebSocket;
    
    public class CommandSubsystem : ICommandSubsystem
    {
        private IConfiguration config;
        private DiscordModel discord_config;
        private ILogger log;
        private List<BaseCommand> commands;
        
        public CommandSubsystem(IConfiguration configuration, ILogger<CommandSubsystem> logger)
        {
            config         = configuration;
            discord_config = DiscordModel.FromConfiguration(configuration);
            log            = logger;
            commands       = new List<BaseCommand>();
        }
        
        
        
        public void Dispose()
        {
            
        }
    }
}
