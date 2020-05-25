
namespace ServerCS.DiscordHandler.Commands.JavaScriptCommandHandler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Discord.WebSocket;
    
    public class JsDiscordMessage
    {
        public ulong Id { get; set; }
        public string Content { get; set; } = string.Empty;
        
        public JsDiscordMessage()
        {
            
        }
        
        public JsDiscordMessage(SocketMessage message)
        {
            Id      = message.Id;
            Content = message.Content;
        }
        
        public static JsDiscordMessage FromSocketMessage(SocketMessage discord_message)
        {
            return new JsDiscordMessage(discord_message);
        }
    }
}
