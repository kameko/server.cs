
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
    
    // TODO: get javascript command paths (multiple) from config
    
    public class JavaScriptCommand : Commands.CompleteCommandHandler
    {
        private List<JsScriptContainer> scripts;
        
        public JavaScriptCommand(ILogger logger, CommandSubsystem commands)
            : base(nameof(JavaScriptCommand), logger, commands)
        {
            scripts = new List<JsScriptContainer>();
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
        
        private class JsScriptContainer
        {
            public Engine JsEngine { get; set; }
            
            public JsScriptContainer()
            {
                JsEngine = new Engine();
            }
        }
    }
}
