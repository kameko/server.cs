
namespace ServerCS.DiscordHandler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Runtime.CompilerServices;
    using Discord;
    using Discord.WebSocket;
    using Discord.Rest;
    
    public class ChannelHandle
    {
        private DiscordSocketClient client;
        private SocketTextChannel channel;
        
        public string Name      => channel.Name;
        public ulong Id         => channel.Id;
        public string Topic     => channel.Topic;
        public int MemberCount  => channel.Users.Count;
        
        public string GuildName     => channel.Guild.Name;
        public ulong GuildId        => channel.Guild.Id;
        public int GuildMemberCount => channel.Guild.Users.Count;
        
        internal ChannelHandle(DiscordSocketClient discord_client, SocketTextChannel text_channel)
        {
            client  = discord_client;
            channel = text_channel;
        }
        
        public async Task<RestUserMessage> Send(OutgoingMessage message)
        {
            // if (message.Content.Length > DiscordConfig.MaxMessageSize)
            
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
        
        public async IAsyncEnumerable<IMessage> GetMessages(
            [EnumeratorCancellation] CancellationToken token,
            FetchMessagesOptions? options = null
        )
        {
            options ??= FetchMessagesOptions.Default;
            
            var messages = options.FromMessageId > 0
                ? await channel.GetMessagesAsync(
                        fromMessageId : options.FromMessageId,
                        dir           : options.Direction,
                        limit         : options.Limit,
                        options       : options.RequestOptions
                    ).FlattenAsync()
                : await channel.GetMessagesAsync(
                        limit   : options.Limit,
                        options : options.RequestOptions
                    ).FlattenAsync();
            
            while (messages.Count() > 0 && !token.IsCancellationRequested)
            {
                foreach (var message in messages)
                {
                    if (token.IsCancellationRequested)
                    {
                        yield break;
                    }
                    
                    yield return message;
                }
                
                var last = messages.LastOrDefault();
                
                if (last is null)
                {
                    yield break;
                }
                
                try
                {
                    await Task.Delay(options.Delay);
                }
                catch (TaskCanceledException)
                {
                    yield break;
                }
                
                messages = await channel.GetMessagesAsync(
                    fromMessageId : options.FromMessageId,
                    dir           : options.Direction,
                    limit         : options.Limit,
                    options       : options.RequestOptions
                ).FlattenAsync();
                
                // TODO: we probably don't need this, but check if we do anyway.
                if (messages.Count() == 0)
                {
                    yield break;
                }
            }
        }
        
        public class FetchMessagesOptions
        {
            public static readonly FetchMessagesOptions Default = new FetchMessagesOptions();
            
            public int Delay { get; set; } = 100;
            public int Limit { get; set; } = 1_000;
            public ulong FromMessageId { get; set; } = 0;
            public Direction Direction { get; set; } = Direction.Around;
            public RequestOptions? RequestOptions { get; set; }
        }
    }
}
