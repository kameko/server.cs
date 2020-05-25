
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
    
    public abstract class Command : IDisposable
    {
        public string Name { get; protected set; }
        protected ILogger Log { get; private set; }
        protected ICommandSubsystem CommandSubsystem { get; private set; }
        
        public Command(string name, ILogger logger, ICommandSubsystem commands)
        {
            Name                 = name;
            Log                  = logger;
            CommandSubsystem     = commands;
        }
        
        public virtual Task Setup() => Task.CompletedTask;
        public virtual void Dispose() { /* pass */ }
    }
}
