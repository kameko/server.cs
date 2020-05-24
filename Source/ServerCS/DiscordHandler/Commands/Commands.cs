
namespace ServerCS.DiscordHandler.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Standard.Logging;
    using Discord;
    using Discord.WebSocket;
    
    public static class Commands
    {
        public abstract class ClientReadyCommand : Command
        {
            public abstract Task<Result> Process();
            
            public class Result : CommandResult
            {
                
            }
        }
        
        public abstract class MessageReceivedCommand : Command
        {
            public abstract Task<Result> Process(SocketMessage message);
            
            public class Result : CommandResult
            {
                
            }
        }
        
        public abstract class MessageUpdatedCommand : Command
        {
            public abstract Task<Result> Process(SocketMessage old_message, SocketMessage new_message);
            
            public class Result : CommandResult
            {
                
            }
        }
    }
}
