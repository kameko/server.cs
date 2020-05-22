
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
            if (message.File is null)
            {
                return await channel.SendMessageAsync(
                    text    : message.Content,
                    isTTS   : message.IsTTS,
                    embed   : message.Embed,
                    options : message.RequestOptions
                );
            }
            else
            {
                return await channel.SendFileAsync(
                    stream    : message.File.Stream,
                    filename  : message.File.FileName,
                    text      : message.Content,
                    isTTS     : message.IsTTS,
                    embed     : message.Embed,
                    options   : message.RequestOptions,
                    isSpoiler : message.File.Spoiler
                );
            }
        }
        
        // TODO: iterate over all messages
    }
}
