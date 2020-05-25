
namespace ServerCS.DiscordHandler.Commands.JavaScriptCommandHandler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Discord.WebSocket;
    using Jint;
    using Jint.Native;
    using ConfigurationModels;
    using Services.JavaScriptHost;
    
    public class JavaScriptCommand : Commands.CompleteCommandHandler
    {
        private JsStorage global_storage;
        private List<JsEngineContainer> scripts;
        
        public JavaScriptCommand(ILogger logger, CommandSubsystem commands)
            : base(nameof(JavaScriptCommand), logger, commands)
        {
            global_storage = new JsStorage();
            scripts        = new List<JsEngineContainer>();
        }
        
        public override Task Setup()
        {
            var config = CommandSubsystem.DiscordConfiguration;
            // TODO: get javascript command paths (multiple) from config.
            // DirectoryInfo.GetFiles returns FileInfo[].
            // Make sure to check for the .js extension.
            
            return Task.CompletedTask;
        }
        
        public override Task<Result> ProcessClientReady() => 
            GeneralProcessor("on_client_ready");
        
        public override Task<Result> ProcessReceivedMessage(SocketMessage message) =>
            GeneralProcessor(
                "on_message_received",
                JsDiscordMessage.FromSocketMessage(message)
            );
        
        public override Task<Result> ProcessUpdatedMessage(SocketMessage old_message, SocketMessage new_message) =>
            GeneralProcessor(
                "on_message_updated",
                JsDiscordMessage.FromSocketMessage(old_message),
                JsDiscordMessage.FromSocketMessage(new_message)
            );
        
        private Task<Result> GeneralProcessor(string func_name, params object[]? args)
        {
            var stop_processing = false;
            
            foreach (var container in scripts)
            {
                var result = container.Call(func_name, args);
                if (result.IsBoolean() && !stop_processing)
                {
                    stop_processing = result.AsBoolean();
                }
            }
            
            return stop_processing ? StopProcessing() : Continue();
        }
    }
}
