
namespace ServerCS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Standard.Logging;
    using Services;
    
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging
                        .ClearProviders()
                        .AddCaesuraConsoleLogger(config =>
                        {
                            config.Token = LifetimeEventsHostedService.Token;
                            config.LogLevel  = LogLevel.Trace;
                            config.TrimNames = new List<string>()
                            {
                                "ServerCS.Controllers.",
                                "ServerCS.",
                                "System.Net.Http.",
                            };
                        });
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
