
namespace ServerCS.DiscordHandler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.IO;
    
    public class File
    {
        public string FileName { get; set; }
        public Stream Stream { get; set; }
        public bool Spoiler { get; set; }
        
        public File(string filename, Stream stream, bool spoiler)
        {
            FileName = filename;
            Stream   = stream;
            Spoiler  = spoiler;
        }
        
        public File(string filename, Stream stream) : this(filename, stream, false) { }
    }
}
