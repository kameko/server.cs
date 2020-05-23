
namespace ServerCS.ConfigurationModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    
    public class ServerCSModel
    {
        public static string ConfigurationKey { get; set; } = "ServerCS";
        
        public string DataFolder { get; set; } = string.Empty;
        
        public static ServerCSModel FromConfiguration(IConfiguration configuration)
        {
            return configuration.GetSection(ConfigurationKey).Get<ServerCSModel>();
        }
    }
}
