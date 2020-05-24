
namespace ServerCS.ConfigurationModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    
    public class DiscordModel
    {
        public static string ConfigurationKey { get; set; } = "Discord";
        
        public string TokenFilePath { get; set; } = string.Empty;
        public IDictionary<string, JavaScriptModel> JavaScriptHostOptions { get; set; } = new Dictionary<string, JavaScriptModel>();
        
        public static DiscordModel FromConfiguration(IConfiguration configuration)
        {
            return configuration.GetSection(ConfigurationKey).Get<DiscordModel>();
        }
    }
}
