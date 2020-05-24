
namespace ServerCS.DiscordHandler.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Standard.Logging;
    using Discord;
    using Discord.WebSocket;
    
    public abstract class Command : IDisposable
    {
        public virtual void Dispose() { /* pass */ }
    }
}
