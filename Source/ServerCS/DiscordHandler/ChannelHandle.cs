
namespace ServerCS.DiscordHandler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.IO;
    using System.Threading.Tasks;
    using Discord;
    using Discord.WebSocket;
    using Discord.Rest;
    
    public class ChannelHandle
    {
        private DiscordSocketClient client;
        private SocketTextChannel channel;
        
        internal ChannelHandle(DiscordSocketClient discord_client, SocketTextChannel text_channel)
        {
            client  = discord_client;
            channel = text_channel;
        }
        
        public async Task<RestUserMessage> Send(OutgoingMessage message)
        {
            return await channel.SendMessageAsync(
                text    : message.Content,
                embed   : message.Embed,
                options : message.RequestOptions
            );
        }
        
        public async Task<RestUserMessage> SendFile(File file, OutgoingMessage message)
        {
            return await channel.SendFileAsync(
                stream    : file.Stream,
                filename  : file.FileName,
                text      : message.Content,
                embed     : message.Embed,
                options   : message.RequestOptions,
                isSpoiler : file.Spoiler
            );
        }
    }
}
