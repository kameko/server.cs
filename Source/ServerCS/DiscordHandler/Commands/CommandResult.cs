
namespace ServerCS.DiscordHandler.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    
    // TODO: return some kind of object that says which commands,
    // if any, can run after this. For that we should have a decent
    // way to register command names/IDs.
    
    public abstract class CommandResult
    {
        public bool StopProcessingCommands { get; protected set; }
    }
}
