
namespace ServerCS.DiscordHandler.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Discord.WebSocket;
    using ConfigurationModels;
    
    public interface ICommandSubsystem : IDisposable
    {
        /// <summary>
        /// This is set externally by the DiscordClient after it has finished initializing.
        /// Do not call this inside of any constructor, only in callbacks.
        /// </summary>
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
