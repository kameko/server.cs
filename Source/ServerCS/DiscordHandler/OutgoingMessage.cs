
namespace ServerCS.DiscordHandler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Discord;
    
    public class OutgoingMessage : IDisposable, IAsyncDisposable
    {
        public string Content { get; set; }
        public File? File { get; set; }
        public Embed? Embed { get; set; }
        public RequestOptions? RequestOptions { get; set; }
        public bool IsTTS { get; set; }
        
        public OutgoingMessage(string content, File file, Embed embed, RequestOptions request_options)
        {
            Content        = content;
            File           = file;
            Embed          = embed;
            RequestOptions = request_options;
            IsTTS          = false;
        }
        
        public OutgoingMessage(string content, File file, Embed embed)
            : this(content, file, embed, null!) { }
        public OutgoingMessage(string content, File file, RequestOptions request_options)
            : this(content, file, null!, request_options) { }
        public OutgoingMessage(string content, File file)
            : this(content, file, null!, null!) { }
        public OutgoingMessage(string content, Embed embed, RequestOptions request_options)
            : this(content, null!, embed, request_options) { }
        public OutgoingMessage(string content, Embed embed)
            : this(content, null!, embed, null!) { }
        public OutgoingMessage(string content, RequestOptions request_options)
            : this(content, null!, null!, request_options) { }
        public OutgoingMessage(string content)
            : this(content, null!, null!, null!) { }
        
        public static implicit operator OutgoingMessage (string content) => new OutgoingMessage(content);
        
        public void Dispose()
        {
            File?.Dispose();
        }
        
        public ValueTask DisposeAsync()
        {
            if (File is null)
            {
                return new ValueTask(Task.CompletedTask);
            }
            
            return File.DisposeAsync();
        }
    }
}
