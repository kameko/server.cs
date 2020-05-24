
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
    using Jint.Runtime;
    using ConfigurationModels;
    using Services.JavaScriptHost;
    
    // TODO: get javascript command paths (multiple) from config.
    // DirectoryInfo.GetFiles returns FileInfo[].
    // Make sure to check for the .js extension.
    
    public class JavaScriptCommand : Commands.CompleteCommandHandler
    {
        private List<JsEngineContainer> scripts;
        
        public JavaScriptCommand(ILogger logger, CommandSubsystem commands)
            : base(nameof(JavaScriptCommand), logger, commands)
        {
            scripts = new List<JsEngineContainer>();
        }
        
        public override Task<Result> ProcessClientReady()
        {
            throw new NotImplementedException();
        }
        
        public override Task<Result> ProcessReceivedMessage(SocketMessage message)
        {
            throw new NotImplementedException();
        }
        
        public override Task<Result> ProcessUpdatedMessage(SocketMessage old_message, SocketMessage new_message)
        {
            throw new NotImplementedException();
        }
    }
}
