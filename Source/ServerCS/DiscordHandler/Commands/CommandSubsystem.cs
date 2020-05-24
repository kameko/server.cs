
namespace ServerCS.DiscordHandler.Commands
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Standard.Logging;
    using Discord;
    using Discord.WebSocket;
    using ConfigurationModels;
    using JavaScriptCommandHandler;
    
    public class CommandSubsystem : ICommandSubsystem
    {
        /// <summary>
        /// This is set externally by the DiscordClient after it has finished initializing.
        /// Do not call this inside of any constructor, only in callbacks.
        /// </summary>
        public DiscordClient Client { get; private set; }
        public CommandRegistry Registry { get; private set; }
        public DiscordModel DiscordConfiguration { get; private set; }
        
        private IConfiguration config;
        private ILogger log;
        private ILogger command_log;
        private ConcurrentDictionary<string, Command> commands;
        
        public CommandSubsystem(IConfiguration configuration, ILogger<CommandSubsystem> logger, ILogger<Command> command_logger)
        {
            Client               = null!;
            Registry             = new CommandRegistry();
            DiscordConfiguration = DiscordModel.FromConfiguration(configuration);
            
            config         = configuration;
            log            = logger;
            command_log    = command_logger;
            commands       = new ConcurrentDictionary<string, Command>();
            
            AddCommands();
        }
        
        private void AddCommands()
        {
            AddCommand((logger, self) => new JavaScriptCommand(logger, self));
        }
        
        public void SetClient(DiscordClient client)
        {
            if (!(client is null))
            {
                Client = client;
            }
        }
        
        public Task Start()
        {
            return Task.CompletedTask;
        }
        
        public Task Stop()
        {
            return Task.CompletedTask;
        }
        
        public bool AddCommand(Func<ILogger, CommandSubsystem, Command> command_factory)
        {
            var command = command_factory.Invoke(command_log, this);
            var success = commands.TryAdd(command.Name, command);
            return success;
        }
        
        public async Task ProcessOnClientReady()
        {
            var should_continue = await GeneralProcessor<Commands.ClientReadyCommand>(async command =>
                await command.Process()
            );
            
            if (should_continue)
            {
                await GeneralProcessor<Commands.CompleteCommandHandler>(async command =>
                    await command.ProcessClientReady()
                );
            }
        }
        
        public async Task ProcessMessageReceived(SocketMessage message)
        {
            var should_continue = await GeneralProcessor<Commands.MessageReceivedCommand>(async command =>
                await command.Process(message)
            );
            
            if (should_continue)
            {
                await GeneralProcessor<Commands.CompleteCommandHandler>(async command =>
                    await command.ProcessReceivedMessage(message)
                );
            }
        }
        
        public async Task ProcessMessageUpdated(SocketMessage old_message, SocketMessage new_message)
        {
            var should_continue = await GeneralProcessor<Commands.MessageUpdatedCommand>(async command =>
                await command.Process(old_message, new_message)
            );
            
            if (should_continue)
            {
                await GeneralProcessor<Commands.CompleteCommandHandler>(async command =>
                    await command.ProcessUpdatedMessage(old_message, new_message)
                );
            }
        }
        
        public void Dispose()
        {
            foreach (var (name, command) in commands)
            {
                command.Dispose();
            }
        }
        
        private async Task<bool> GeneralProcessor<T>(Func<T, Task<CommandResult>> callback)
        {
            foreach (var (name, base_command) in commands)
            {
                try
                {
                    if (base_command is T command)
                    {
                        var result = await callback.Invoke(command);
                        if (result.StopProcessingCommands)
                        {
                            return false;
                        }
                    }
                }
                catch (Exception e)
                {
                    log.Error(e, $"Exception encountered when processing command \"{name}\".");
                }
            }
            
            return true;
        }
    }
}
