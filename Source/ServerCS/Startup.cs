
namespace ServerCS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.HttpsPolicy;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Caesura.Logging;
    using Services;
    using DiscordHandler;
    
    public class Startup
    {
        private IConfiguration config;
        private ServiceProvider serviceProvider;
        
        public Startup(IConfiguration configuration)
        {
            config = configuration;
            serviceProvider = null!;
        }
        
        private Task Run(ILogger log, DiscordClient discord)
        {
            
            return Task.CompletedTask;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ConsoleLifetimeOptions>(opts => opts.SuppressStatusMessages = true);
            
            services.AddHostedService<LifetimeEventsHostedService>();
            
            services.AddSingleton<IDiscordClient, DiscordClient>();
            services.AddSingleton<Runtime>();
            
            services.AddControllers();
            
            serviceProvider = services.BuildServiceProvider();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> log)
        {
            app.UseDeveloperExceptionPage();
            
            //app.UseHttpsRedirection();
            
            app.UseRouting();
            
            //app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            var runtime = ActivatorUtilities.CreateInstance<Runtime>(serviceProvider);
            runtime.Start();
        }
    }
}
