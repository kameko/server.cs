
namespace ServerCS.DiscordHandler
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Standard.Logging;
    using Services;
    using ConfigurationModels;
    using Discord;
    using Discord.WebSocket;
    
    public class DiscordClient : IDiscordClient
    {
        private IConfiguration config;
        private DiscordModel discord_config;
        private ILogger log;
        private CancellationTokenSource cts;
        private DiscordSocketClient client;
        
        private ConcurrentQueue<SocketMessage> message_queue;
        private ConcurrentQueue<(SocketMessage Old, SocketMessage New)> message_updated_queue;
        
        public DiscordModel DiscordConfiguration => discord_config;
        
        /// <summary>
        /// Invoked when the client encountered an internal error, usually due to a callback
        /// throwing an exception. Invoke the provided Action if you want the client to shutdown.
        /// </summary>
        public event Func<Exception, Action, Task> OnError;
        /// <summary>
        /// Invoked when a new message has been recieved.
        /// </summary>
        public event Func<SocketMessage, Task> OnMessageReceived;
        /// <summary>
        /// The first SocketMessage is the old message, the second SocketMessage is the
        /// current updated message. This event will not fire if the old message has not
        /// been cached.
        /// </summary>
        public event Func<SocketMessage, SocketMessage, Task> OnMessageUpdated;
        
        public DiscordClient(IConfiguration configuration, ILogger<DiscordClient> logger)
        {
            config         = configuration;
            discord_config = DiscordModel.FromConfiguration(configuration);
            log            = logger;
            cts            = new CancellationTokenSource();
            
            var dsc = new DiscordSocketConfig()
            {
                ExclusiveBulkDelete = true,
                LogLevel            = LogSeverity.Critical,
            };
            
            client = new DiscordSocketClient(dsc);
            
            message_queue         = new ConcurrentQueue<SocketMessage>();
            message_updated_queue = new ConcurrentQueue<(SocketMessage Old, SocketMessage New)>();
            
            OnError           = delegate { return Task.CompletedTask; };
            OnMessageReceived = delegate { return Task.CompletedTask; };
            OnMessageUpdated  = delegate { return Task.CompletedTask; };
            
            SetupEventCallbacks();
            
            log.InstanceAbreaction();
        }
        
        public async Task Start(string token)
        {
            cts = new CancellationTokenSource();
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            StartEventLoop();
        }
        
        public async Task Stop()
        {
            cts.Cancel();
            await client.LogoutAsync();
            await client.StopAsync();
        }
        
        public bool IsInGuild(ulong guild_id)
        {
            return !(client.GetGuild(guild_id) is null);
        }
        
        public bool ChannelExists(ulong guild_id, ulong channel_id)
        {
            return !(client.GetGuild(guild_id)?.GetChannel(channel_id) is null);
        }
        
        public bool TryGetChannel(ulong guild_id, ulong channel_id, ChannelHandle? handle)
        {
            var channel = client.GetGuild(guild_id)?.GetTextChannel(channel_id);
            
            if (channel is null)
            {
                handle = null;
                return false;
            }
            
            handle = new ChannelHandle(client, channel);
            return true;
        }
        
        public void Dispose()
        {
            client.Dispose();
        }
        
        // --- Private Methods --- //
        
        private bool IsCancelled() =>
            cts.IsCancellationRequested || LifetimeEventsHostedService.Token.IsCancellationRequested;
        
        private void StartEventLoop() => Task.Run(EventLoop);
        
        private async Task EventLoop()
        {
            while (!IsCancelled())
            {
                try
                {
                    await HandleReceivedMessages(); // always run this one first due to it's delay.
                    await HandleUpdatedMessages();
                }
                catch (Exception e)
                {
                    var kill = false;
                    
                    try
                    {
                        Action poison_pill = () => kill = true;
                        await OnError.Invoke(e, poison_pill);
                    }
                    catch (Exception ei)
                    {
                        log.Critical(
                            ei,
                            "Exception thrown when handling exception handler handling exception {ex}.",
                            e
                        );
                        kill = true;
                    }
                    
                    if (kill)
                    {
                        log.Warning(
                            e,
                            "Exception encountered in main event loop. Exception handler "
                          + "has requested shutdown of client."
                        );
                        
                        try
                        {
                            await Stop();
                        }
                        catch (Exception ei)
                        {
                            log.Error(
                                ei,
                                "Exception thrown when attempting to log out of Discord after being "
                              + "requested to shut down due to encountering exception {ex}",
                                e
                            );
                        }
                        
                        break;
                    }
                }
            }
            
            try
            {
                if (LifetimeEventsHostedService.Token.IsCancellationRequested &&
                    !cts.IsCancellationRequested)
                {
                    // System is shutting down but client was no requested to log out,
                    // so attempt logging out here.
                    await Stop();
                }
            }
            catch (Exception e)
            {
                log.Error(
                    e, 
                    "Error when attempting to stop client after "
                  + "LifetimeEventsHostedService's CancellationToken "
                  + "was cancelled."
                );
            }
        }
        
        // --- Callback Buffer Handlers --- //
        
        private async Task HandleReceivedMessages()
        {
            if (message_queue.IsEmpty)
            {
                // The bulk of our workload is going to be receiving new messages,
                // so if there are none, then give the system a rest, and delay for
                // a couple ms before we try seeing if there are any other events
                // to handle.
                await Task.Delay(100);
                return;
            }
            
            while (!IsCancelled())
            {
                var success = message_queue.TryDequeue(out var message);
                if (!success)
                {
                    break;
                }
                
                await OnMessageReceived.Invoke(message!);
            }
        }
        
        private async Task HandleUpdatedMessages()
        {
            if (message_updated_queue.IsEmpty)
            {
                return;
            }
            
            while (!IsCancelled())
            {
                var success = message_updated_queue.TryDequeue(out var update);
                if (!success)
                {
                    break;
                }
                
                await OnMessageUpdated.Invoke(update.Old, update.New);
            }
        }
        
        // --- Discord Client Event Callbacks --- //
        
        private void SetupEventCallbacks()
        {
            client.Log             += OnLog;
            client.Ready           += HandleOnReady;
            client.MessageReceived += OnClientReceiveMessage;
            client.MessageUpdated  += OnClientUpdateMessage;
        }
        
        private Task HandleOnReady()
        {
            var user = client.CurrentUser;
            log.Information($"Logged into Discord as {user.Username}.");
            return Task.CompletedTask;
        }
        
        private Task OnClientReceiveMessage(SocketMessage message)
        {
            if (message.Author.Id != client.CurrentUser.Id)
            {
                message_queue.Enqueue(message);
            }
            return Task.CompletedTask;
        }
        
        private async Task OnClientUpdateMessage(Cacheable<IMessage, ulong> cache, SocketMessage message, ISocketMessageChannel channel)
        {
            try
            {
                var old_get = await cache.GetOrDownloadAsync();
                if (old_get is SocketMessage old && old.Content != message.Content)
                {
                    message_updated_queue.Enqueue((Old: old, New: message));
                }
            }
            catch (NullReferenceException e)
            {
                log.Debug(e, "Ignored exception thrown by Cachable.GetOrDownloadAsync().");
            }
        }
        
        private Task OnLog(LogMessage msg)
        {
            var level = LogSeverityToLogLevel(msg.Severity);
            
            // TODO: check out msg.Source
            return Task.Run(() =>
                log.Raw(level, msg.Exception, msg.Message)
            );
        }
        
        private LogLevel LogSeverityToLogLevel(LogSeverity severity) =>
            severity switch
            {
                LogSeverity.Info     => LogLevel.Information,
                LogSeverity.Warning  => LogLevel.Warning,
                LogSeverity.Error    => LogLevel.Error,
                LogSeverity.Critical => LogLevel.Critical,
                LogSeverity.Debug    => LogLevel.Debug,
                LogSeverity.Verbose  => LogLevel.Trace,
                _ => LogLevel.Information
            };
    }
}
