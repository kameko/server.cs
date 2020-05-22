
namespace ServerCS.DiscordHandler
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.IO;
    
    public interface IDiscordClient
    {
        Task Start(string token);
        Task Stop();
        bool IsInGuild(ulong guild_id);
        bool ChannelExists(ulong guild_id, ulong channel_id);
        bool TryGetChannel(ulong guild_id, ulong channel_id, ChannelHandle? handle);
    }
}
