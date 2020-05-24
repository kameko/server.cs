
namespace ServerCS.DiscordHandler.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Discord.WebSocket;
    
    public interface ICommandSubsystem : IDisposable
    {
        Task Start();
        Task Stop();
        Task ProcessOnClientReady();
        Task ProcessMessageReceived(SocketMessage message);
        Task ProcessMessageUpdated(SocketMessage old_message, SocketMessage new_message);
    }
}
