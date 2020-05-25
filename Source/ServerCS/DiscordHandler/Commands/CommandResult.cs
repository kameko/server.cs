
namespace ServerCS.DiscordHandler.Commands
{
    using System;
    
    public abstract class CommandResult
    {
        public bool StopProcessingCommands { get; set; }
        
        public CommandResult()
        {
            
        }
    }
}
