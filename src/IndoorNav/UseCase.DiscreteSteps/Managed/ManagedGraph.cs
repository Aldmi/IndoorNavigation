using System;
using System.Reactive.Concurrency;
using Libs.Beacons;
using Microsoft.Extensions.Logging;

namespace UseCase.DiscreteSteps.Managed
{
    public class ManagedGraph: IDisposable
    {
        private readonly IBeaconRangingManager _beaconManager;
        private readonly ILogger? _logger;
        private IScheduler? _scheduler;
        private IDisposable? _scanSub;
        
        
        
        public void Dispose()
        {
        }
    }
}