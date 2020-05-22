
namespace ServerCS.DiscordHandler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Discord;
    
    public class OutgoingMessage
    {
        public string Content { get; set; }
        public Embed? Embed { get; set; }
        public RequestOptions? RequestOptions { get; set; }
        
        public OutgoingMessage(string content, Embed embed, RequestOptions request_options)
        {
            Content        = content;
            Embed          = embed;
            RequestOptions = request_options;
        }
        
        public OutgoingMessage(string content, Embed embed) : this(content, embed, null!) { }
        public OutgoingMessage(string content) : this(content, null!, null!) { }
        
        public static implicit operator OutgoingMessage (string content) => new OutgoingMessage(content);
    }
}
