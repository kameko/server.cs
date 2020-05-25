
namespace ServerCS.DiscordHandler.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Standard.Logging;
    using Discord;
    using Discord.WebSocket;
    
    public static class Commands
    {
        public abstract class CompleteCommandHandler : Command
        {
            public CompleteCommandHandler(string name, ILogger logger, ICommandSubsystem commands)
                : base(name, logger, commands)
            {
                
            }
            
            public abstract Task<Result> ProcessClientReady();
            public abstract Task<Result> ProcessReceivedMessage(SocketMessage message);
            public abstract Task<Result> ProcessUpdatedMessage(SocketMessage old_message, SocketMessage new_message);
            
            protected Task<Result> StopProcessing() => Task.FromResult(new Result() { StopProcessingCommands = true });
            protected Task<Result> Continue() => Task.FromResult(new Result() { StopProcessingCommands = false });
            
            public class Result : CommandResult
            {
                
            }
        }
        
        public abstract class ClientReadyCommand : Command
        {
            public ClientReadyCommand(string name, ILogger logger, ICommandSubsystem commands)
                : base(name, logger, commands)
            {
                
            }
            
            public abstract Task<Result> Process();
            
            protected Task<Result> StopProcessing() => Task.FromResult(new Result() { StopProcessingCommands = true });
            protected Task<Result> Continue() => Task.FromResult(new Result() { StopProcessingCommands = false });
            
            public class Result : CommandResult
            {
                
            }
        }
        
        public abstract class MessageReceivedCommand : Command
        {
            public MessageReceivedCommand(string name, ILogger logger, ICommandSubsystem commands)
                : base(name, logger, commands)
            {
                
            }
            
            public abstract Task<Result> Process(SocketMessage message);
            
            protected Task<Result> StopProcessing() => Task.FromResult(new Result() { StopProcessingCommands = true });
            protected Task<Result> Continue() => Task.FromResult(new Result() { StopProcessingCommands = false });
            
            public class Result : CommandResult
            {
                
            }
        }
        
        public abstract class MessageUpdatedCommand : Command
        {
            public MessageUpdatedCommand(string name, ILogger logger, ICommandSubsystem commands)
                : base(name, logger, commands)
            {
                
            }
            
            public abstract Task<Result> Process(SocketMessage old_message, SocketMessage new_message);
            
            protected Task<Result> StopProcessing() => Task.FromResult(new Result() { StopProcessingCommands = true });
            protected Task<Result> Continue() => Task.FromResult(new Result() { StopProcessingCommands = false });
            
            public class Result : CommandResult
            {
                
            }
        }
    }
}
