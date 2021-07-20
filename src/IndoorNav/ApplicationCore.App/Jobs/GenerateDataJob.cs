using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.App.Infrastructure;
using ApplicationCore.App.PlatformServices;
using Shiny;
using Shiny.Jobs;
using Xamarin.Forms;


namespace ApplicationCore.App.Jobs
{
    public class GenerateDataJob : IJob, IShinyStartupTask
    {
        private readonly CoreDelegateServices _services;
        private readonly IMessageBus _messageBus;
        
        public GenerateDataJob(CoreDelegateServices services, IMessageBus messageBus)
        {
            _services = services;
            _messageBus = messageBus;
        }
        
        public async Task Run(JobInfo jobInfo, CancellationToken cancelToken)
        {
            for (int i = 0; i < 1000; i++)
            { 
                cancelToken.ThrowIfCancellationRequested(); 
                
               await Task.Delay(1000, cancelToken);
               Debug.WriteLine($"Generate !!!!!!!!!!   {DateTime.Now:T}");
               _messageBus.Publish(new BleData{Id = i, Name = "jyfhghkghjjf"});
            }
            
            //
            // await _notifications.Send(
            //     "Job Started",
            //     $"{jobInfo.Identifier} Started"
            // );
            // var seconds = jobInfo.Parameters.Get("SecondsToRun", 10);
            // await Task.Delay(TimeSpan.FromSeconds(seconds), cancelToken);
            //
            // await _notifications.Send(
            //     "Job Finished",
            //     $"{jobInfo.Identifier} Finished"
            // );
        }

        public void Start()
            => _services.Notifications.RegisterInit(GetType(), true, "Jobs");
        
    }

    public class BleData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Rssi { get; set; }
    }
}
