
namespace ServerCS.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    
    public class LifetimeEventsHostedService : IHostedService
    {
        public static bool Started { get; private set; } = false;
        public static event Action OnStarted  = delegate { };
        public static event Action OnStopping = delegate { };
        public static event Action OnStopped  = delegate { };
        public static CancellationToken Token => tokenSource.Token;
        
        private static CancellationTokenSource tokenSource = new CancellationTokenSource();
        private static Action? end_application_callback;
        private readonly IHostApplicationLifetime _appLifetime;
        
        public LifetimeEventsHostedService(IHostApplicationLifetime appLifetime)
        {
            _appLifetime = appLifetime;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(CallOnStarted);
            _appLifetime.ApplicationStopping.Register(CallOnStopping);
            _appLifetime.ApplicationStopped.Register(CallOnStopped);
            
            if (end_application_callback is null)
            {
                end_application_callback = () => _appLifetime.StopApplication();
            }
            
            Started = true;
            
            return Task.CompletedTask;
        }
        
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        
        public static void StopApplication()
        {
            if (end_application_callback is null)
            {
                throw new InvalidOperationException("Callback to end application is not set.");
            }
            
            end_application_callback.Invoke();
        }
        
        private void CallOnStarted()
        {
            OnStarted.Invoke();
        }
        
        private void CallOnStopping()
        {
            tokenSource.Cancel();
            OnStopping.Invoke();
        }
        
        private void CallOnStopped()
        {
            OnStopped.Invoke();
        }
    }
}
