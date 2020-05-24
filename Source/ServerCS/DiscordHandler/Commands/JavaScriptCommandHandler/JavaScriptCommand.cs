
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
    
    // TODO: get javascript command paths (multiple) from config.
    // DirectoryInfo.GetFiles returns FileInfo[].
    // Make sure to check for the .js extension.
    
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
            public FileInfo FileSource { get; set; }
            public Engine JsEngine { get; set; }
            
            public JsScriptContainer(FileInfo file)
            {
                FileSource = file;
                JsEngine   = new Engine(options =>
                {
                    // https://github.com/sebastienros/jint/blob/dev/Jint/Options.cs 
                    
                    /*
                    Hardcode:
                    public Options AddObjectConverter(IObjectConverter objectConverter);
                    public Options Constraint(IConstraint constraint);
                    public Options CatchClrExceptions(Predicate<Exception> handler);
                    
                    Use sane default:
                    public Options RegexTimeoutInterval(TimeSpan regexTimeoutInterval);
                    public Options Culture(CultureInfo cultureInfo);
                    */
                    
                    // CONSIDER: public Options SetWrapObjectHandler(Func<Engine, object, ObjectInstance> wrapObjectHandler);
                    // CONSIDER: public Options SetReferencesResolver(IReferenceResolver resolver);
                    // NOTICE: JavaScriptModel.Debug is for AllowDebuggerStatement, not DebugMode (which is always true).
                });
            }
        }
    }
}
