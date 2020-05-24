
namespace ServerCS.DiscordHandler
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.IO;
    using ConfigurationModels;
    using Discord.WebSocket;
    
    public interface IDiscordClient : IDisposable
    {
        DiscordModel DiscordConfiguration { get; }
        
        event Func<Exception, Action, Task> OnError;
        event Func<SocketMessage, Task> OnMessageReceived;
        event Func<SocketMessage, SocketMessage, Task> OnMessageUpdated;
        
        Task Start(string token);
        Task Stop();
        bool IsInGuild(ulong guild_id);
        bool ChannelExists(ulong guild_id, ulong channel_id);
        bool TryGetChannel(ulong guild_id, ulong channel_id, ChannelHandle? handle);
    }
}
