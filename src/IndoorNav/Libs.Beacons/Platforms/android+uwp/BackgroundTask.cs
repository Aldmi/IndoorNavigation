using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Libs.Beacons.Models;
using Libs.BluetoothLE;
using Microsoft.Extensions.Logging;
using Shiny;

namespace Libs.Beacons.Platforms
{
    public class BackgroundTask
    {
        private readonly IBleManager _bleManager;
        private readonly IBeaconMonitoringManager _beaconManager;
        private readonly ILogger _logger;
        private readonly IMessageBus _messageBus;
        private readonly IEnumerable<IBeaconMonitorDelegate> _delegates;
        private readonly IDictionary<string, BeaconRegionStatus> _states;
        private IDisposable? _scanSub;


        public BackgroundTask(IMessageBus messageBus,
                              IBleManager centralManager,
                              IBeaconMonitoringManager beaconManager,
                              IEnumerable<IBeaconMonitorDelegate> delegates,
                              ILogger<IBeaconMonitorDelegate> logger)
        {
            _messageBus = messageBus;
            _bleManager = centralManager;
            _beaconManager = beaconManager;
            _logger = logger;
            _delegates = delegates;
            _states = new Dictionary<string, BeaconRegionStatus>();
        }


        public void Run()
        {
            _logger.LogInformation("Starting Sphere Monitoring");

            // I record state of the beacon region so I can fire stuff without going into initial state from unknown
            _messageBus
                .Listener<BeaconRegisterEvent>()
                .Subscribe(ev =>
                {
                    switch (ev.Type)
                    {
                        case BeaconRegisterEventType.Add:
                            if (_states.Count == 0)
                            {
                                StartScan();
                            }
                            else
                            {
                                lock (_states)
                                {
                                    _states.Add(ev.Region.Identifier, new BeaconRegionStatus(ev.Region));
                                }
                            }
                            break;

                        case BeaconRegisterEventType.Update:
                            // this actually shouldn't be allowed
                            break;

                        case BeaconRegisterEventType.Remove:
                            lock (_states)
                            {
                                _states.Remove(ev.Region.Identifier);
                                if (_states.Count == 0)
                                    StopScan();
                            }
                            break;

                        case BeaconRegisterEventType.Clear:
                            StopScan();
                            break;
                    }
                });

            StartScan();
            _logger.LogInformation("Sphere Monitoring Started Successfully");
        }


        public async void StartScan()
        {
            if (_scanSub != null)
                return;

            _logger.LogInformation("Sphere Monitoring Scan Starting");
            var regions = await _beaconManager.GetMonitoredRegions();
            if (!regions.Any())
                return;

            foreach (var region in regions)
                _states.Add(region.Identifier, new BeaconRegionStatus(region));

            try
            {
                _scanSub = _bleManager
                    .ScanForBeacons(BleScanType.LowPowered)
                    .Buffer(TimeSpan.FromSeconds(5))
                    .SubscribeAsyncConcurrent(CheckStates);

                _logger.LogInformation("Sphere Monitoring Scan Started Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sphere Monitoring Scan Starting");
            }
        }


        private IList<BeaconRegionStatus> GetCopy()
        {
            lock (_states)
                return _states
                    .Select(x => x.Value)
                    .ToList();
        }


        public void StopScan()
        {
            if (_scanSub == null)
                return;

            _scanSub?.Dispose();
            _states.Clear();
            _scanSub = null;
            lock (_states)
                _states.Clear();

            _logger.LogInformation("Sphere Monitoring Scan Stopped");
        }


        private async Task CheckStates(IList<Beacon> beacons)
        {
            var copy = GetCopy();

            foreach (var state in copy)
            {
                foreach (var beacon in beacons)
                {
                    if (state.Region.IsBeaconInRegion(beacon))
                    {
                        state.LastPing = DateTime.UtcNow;
                        state.IsInRange ??= true;

                        if (!state.IsInRange.Value)
                        {
                            state.IsInRange = true;
                            if (state.Region.NotifyOnEntry)
                            {
                                await _delegates.RunDelegates(
                                    x => x.OnStatusChanged(BeaconRegionState.Entered, state.Region)
                                );
                            }
                        }
                    }
                }
            }

            var cutoffTime = DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(20));
            foreach (var state in copy)
            {
                if ((state.IsInRange ?? false) && state.LastPing < cutoffTime)
                {
                    state.IsInRange = false;
                    if (state.Region.NotifyOnExit)
                    {
                        await _delegates.RunDelegates(
                            x => x.OnStatusChanged(BeaconRegionState.Exited, state.Region)
                        );
                    }
                }
            }
        }
    }
}
