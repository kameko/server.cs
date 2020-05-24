
namespace ServerCS.DiscordHandler.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Discord.WebSocket;
    using ConfigurationModels;
    
    public interface ICommandSubsystem : IDisposable
    {
        DiscordClient Client { get; }
        CommandRegistry Registry { get; }
        DiscordModel DiscordConfiguration { get; }
        
        void SetClient(DiscordClient client);
        Task Start();
        Task Stop();
        Task ProcessOnClientReady();
        Task ProcessMessageReceived(SocketMessage message);
        Task ProcessMessageUpdated(SocketMessage old_message, SocketMessage new_message);
    }
}
