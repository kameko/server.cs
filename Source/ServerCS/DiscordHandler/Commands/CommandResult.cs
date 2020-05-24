
namespace ServerCS.DiscordHandler.Commands
{
    using System;
    
    public abstract class CommandResult
    {
        public bool StopProcessingCommands { get; protected set; }
    }
}
