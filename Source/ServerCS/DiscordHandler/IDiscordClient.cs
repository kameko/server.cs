
namespace ServerCS.DiscordHandler
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    
    public interface IDiscordClient
    {
        Task Start(string token);
        Task Stop();
    }
}
