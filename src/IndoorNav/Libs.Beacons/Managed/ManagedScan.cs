using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Libs.Beacons.Managed.FilterFlow;
using Libs.Beacons.Managed.TrilaterationFlow;
using Libs.Beacons.Models;
using Libs.BluetoothLE;
using Microsoft.Extensions.Logging;
using Shiny;

namespace Libs.Beacons.Managed
{
    public class ManagedScan : IDisposable
    {
        private readonly IBeaconRangingManager _beaconManager;
        private readonly ILogger? _logger;
        private IScheduler? _scheduler;
        private IDisposable? _clearSub;
        private IDisposable? _scanSub;
        
        
        public ManagedScan(IBeaconRangingManager beaconManager, ILogger<ManagedScan> logger)
        {
            _beaconManager = beaconManager;
            _logger = logger;
        }


        public ObservableCollection<ManagedSphere> Spheres { get; } = new ObservableCollection<ManagedSphere>();
        public BeaconRegion? ScanningRegion { get; private set; }
        public bool IsScanning => ScanningRegion != null;


        private TimeSpan? _clearTime;
        public TimeSpan? ClearTime
        {
            get => _clearTime;
            set
            {
                _clearTime = value;
                _clearSub?.Dispose();

                if (value != null)
                {
                    _clearSub = Observable
                        .Interval(TimeSpan.FromSeconds(5))
                        .ObserveOnIf(_scheduler)
                        .Synchronize(Spheres)
                        .Subscribe(_ =>
                        {
                            var maxAge = DateTimeOffset.UtcNow.Subtract(value.Value);
                            var tmp = Spheres.Where(x => x.LastSeen < maxAge).ToList();
                            foreach (var sphere in tmp)
                                Spheres.Remove(sphere);
                        });
                }
            }
        }

        
        public void Start(BeaconRegion scanRegion, IScheduler? scheduler = null)
        {
            if (IsScanning)
                throw new ArgumentException("A beacon scan is already running");

            _scheduler = scheduler;
            ScanningRegion = scanRegion;
            Spheres.Clear();

            // restart clear if applicable
            ClearTime = ClearTime;
            
            _scanSub = _beaconManager
                .WhenBeaconRanged(scanRegion, BleScanType.LowLatency)
                //Фильтр
                .AverageFilterDebug(
                    TimeSpan.FromSeconds(2),
                    rssiList => (int) rssiList.Average(r=>r)
                    )
                //Вычисление сфер
                .CreateEmptySphere()
                .AddRadius(2)
                .AddCenter(new Point(1,1))
                //Обработка
                .ObserveOnIf(_scheduler)
                .Synchronize(Spheres)
                .Subscribe(spheres =>
                {
                    foreach (var sphere in spheres)
                    {
                        var managed = Spheres.FirstOrDefault(x => x.Sphere.Beacon.Equals(sphere.Beacon));
                        if (managed == null)
                        {
                            managed = new ManagedSphere(sphere);
                            Spheres.Add(managed);
                        }
                        managed.LastSeen = DateTimeOffset.UtcNow;
                        managed.Proximity = sphere.Beacon.Proximity;
                        managed.Rssi = sphere.Beacon.Rssi;
                        managed.Center = sphere.Center;
                        managed.Radius = sphere.Radius;
                    }

                    //TODO: можно вычислять местоположение. 
                  // var location= Trilateration.CalcLocation(spheres);
                }, exception =>
                {
                    _logger?.LogError(exception, "Ошибка сканирования");
                });
        }


        public void Stop()
        {
            _clearSub?.Dispose();
            _scanSub?.Dispose();
            _scheduler = null;
            ScanningRegion = null;
        }


        public void Dispose()
        {
            Stop();
            Spheres.Clear();
        }
    }
}
