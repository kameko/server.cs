
namespace ServerCS.DiscordHandler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.IO;
    
    public class DiscordFile : IDisposable, IAsyncDisposable
    {
        public string FileName { get; set; }
        public Stream Stream { get; set; }
        public bool Spoiler { get; set; }
        
        public DiscordFile(string filename, Stream stream, bool spoiler)
        {
            FileName = filename;
            Stream   = stream;
            Spoiler  = spoiler;
        }
        
        public DiscordFile(string filename, Stream stream) : this(filename, stream, false) { }
        
        public static DiscordFile FromLocalFile(string path)
        {
            var fi       = new FileInfo(path);
            var filename = fi.Name;
            var stream   = new FileStream(
                path       : path,
                mode       : FileMode.Open,
                access     : FileAccess.Read,
                share      : FileShare.Read,
                bufferSize : 4096,
                useAsync   : true
            );
            return new DiscordFile(filename, stream);
        }
        
        public static DiscordFile FromNetwork(string uri)
        {
            // https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.socket 
            // https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.networkstream 
            throw new NotImplementedException();
        }
        
        public void Dispose()
        {
            Stream.Dispose();
        }
        
        public ValueTask DisposeAsync()
        {
            return Stream.DisposeAsync();
        }
    }
}
