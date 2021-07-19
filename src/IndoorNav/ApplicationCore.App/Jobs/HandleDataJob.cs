using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.App.Infrastructure;
using Shiny;
using Shiny.Jobs;
using Shiny.Notifications;
using IMessageBus = Shiny.IMessageBus;

namespace ApplicationCore.App.Jobs
{
    public class HandleDataJob : IJob, IShinyStartupTask
    {
        private readonly CoreDelegateServices _services;
        private readonly IMessageBus _messageBus;

        public HandleDataJob(CoreDelegateServices services, IMessageBus messageBus)
        {
            _services = services;
            _messageBus = messageBus;
        }
        
        
        public async Task Run(JobInfo jobInfo, CancellationToken cancelToken)
        {
            var gg=_messageBus.HasSubscribers<BleData>();
            var lifeTime= _messageBus.Listener<BleData>().Subscribe(bleData =>
            {
                Debug.WriteLine($"Ha1ndle {bleData.Id}  {DateTime.Now:T}");
            });
            
            // var result = await _notifications.RequestAccess();
            // if (result == AccessState.Available) 
            // {
            //     while (!cancelToken.IsCancellationRequested)
            //     {
            //        await Task.Delay(2000, cancelToken);
            //        Debug.WriteLine("HandleDataJob !!!!!!!!!!");
            //     }
            // }
        }

        public void Start()
            => _services.Notifications.Register(this.GetType(), true, "Jobs");
    }
}
