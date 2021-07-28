using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.App.Infrastructure;
using Libs.Beacons;
using Libs.Beacons.Models;
using Libs.BluetoothLE;
using Microsoft.Extensions.Logging;
using Shiny;
using Shiny.Jobs;


namespace ApplicationCore.App.Jobs
{
    public class BeaconScanJob //: IJob, IShinyStartupTask
    {
        private readonly CoreDelegateServices _services;
        private readonly IBeaconRangingManager _beaconRanging;
        private readonly IMessageBus _messageBus;
        private readonly ILogger? _logger;

        public BeaconScanJob(
            CoreDelegateServices services,
            IBeaconRangingManager beaconRanging,
            IMessageBus messageBus,
            ILogger<BeaconScanJob>? logger)
        {
            _services = services;
            _beaconRanging = beaconRanging;
            _messageBus = messageBus;
            _logger = logger;
        }
        
        /// <summary>
        /// Не работает beaconRanging при спящем режиме.
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task Run(JobInfo jobInfo, CancellationToken cancelToken)
        {
            var access = await _beaconRanging.RequestAccess();
            if (access != AccessState.Available)
            {
                _logger?.LogError($"BeaconScanJob.Run нет разрешения на использование IBeaconRangingManager '{access}'");
                return;
            }
            
            var region = new BeaconRegion(
                "ASSibir_IndorNav",
                Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), //Major=35144, Minor=19824,
                35144,
                19824);
            
            try
            {
                _beaconRanging
                    .WhenBeaconRanged(region, BleScanType.LowLatency)
                    .Select(beacon =>
                    {
                        var extBeacon = new ExtBeacon(beacon);
                        return extBeacon;
                    })
                    .Subscribe(extBeacon =>
                    {
                        //_messageBus.Publish(beacon);
                        Debug.WriteLine(extBeacon);
                    }, exception =>
                    {
              
                    }, () =>
                    {
                            
                    }, cancelToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            //await Task.Delay(-1, cancelToken);

           // await Task.Run(async () =>
           //  {
           //      for (int i = 0; i < 1000; i++)
           //      { 
           //          cancelToken.ThrowIfCancellationRequested(); 
           //      
           //          await Task.Delay(1000, cancelToken);
           //          Debug.WriteLine($"Generate !!!!!!!!!!   {DateTime.Now:T}");
           //          //_logger?.LogWarning("{0}", 10);
           //          //_logger?.LogError("EROR Message {0}  {1}", "RRRTTT", 1);
           //      }
           //  }, cancelToken);

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
}
