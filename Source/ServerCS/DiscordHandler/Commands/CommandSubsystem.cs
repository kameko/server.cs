
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
    
    // TODO: some kind of registry for cross-command data sharing.
    
    public class CommandSubsystem : ICommandSubsystem
    {
        private IConfiguration config;
        private DiscordModel discord_config;
        private ILogger log;
        private List<Command> commands;
        
        public CommandSubsystem(IConfiguration configuration, ILogger<CommandSubsystem> logger)
        {
            config         = configuration;
            discord_config = DiscordModel.FromConfiguration(configuration);
            log            = logger;
            commands       = new List<Command>();
        }
        
        public Task Start()
        {
            return Task.CompletedTask;
        }
        
        public Task Stop()
        {
            return Task.CompletedTask;
        }
        
        public async Task ProcessOnClientReady()
        {
            foreach (var base_command in commands)
            {
                if (base_command is Commands.ClientReadyCommand command)
                {
                    var result = await command.Process();
                    if (result.StopProcessingCommands)
                    {
                        break;
                    }
                }
            }
        }
        
        public async Task ProcessMessageReceived(SocketMessage message)
        {
            foreach (var base_command in commands)
            {
                if (base_command is Commands.MessageReceivedCommand command)
                {
                    var result = await command.Process(message);
                    if (result.StopProcessingCommands)
                    {
                        break;
                    }
                }
            }
        }
        
        public async Task ProcessMessageUpdated(SocketMessage old_message, SocketMessage new_message)
        {
            foreach (var base_command in commands)
            {
                if (base_command is Commands.MessageUpdatedCommand command)
                {
                    var result = await command.Process(old_message, new_message);
                    if (result.StopProcessingCommands)
                    {
                        break;
                    }
                }
            }
        }
        
        public void Dispose()
        {
            foreach (var command in commands)
            {
                command.Dispose();
            }
        }
    }
}
