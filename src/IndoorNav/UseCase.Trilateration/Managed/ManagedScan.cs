using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Libs.Beacons;
using Libs.Beacons.Flows;
using Libs.Beacons.Managed.Domain;
using Libs.Beacons.Models;
using Libs.BluetoothLE;
using Microsoft.Extensions.Logging;
using Shiny;
using UseCase.Trilateration.Flow;
using UseCase.Trilateration.Services;
using Point = Libs.Beacons.Managed.Domain.Point;

namespace UseCase.Trilateration.Managed
{
    public class ManagedScan : IDisposable
    {
        private readonly IBeaconRangingManager _beaconManager;
        private readonly ILogger? _logger;
        private IScheduler? _scheduler;
        private IDisposable? _clearSub;
        private IDisposable? _scanSub;
        private readonly IEnumerable<BeaconOption> _beaconOptions; //TODO: внедрять сервис репозитория, загружать настройки из БД.
        private readonly SphereFactory _sphereFactory;
        
        
        public ManagedScan(IBeaconRangingManager beaconManager, ILogger<ManagedScan> logger)
        {
            _beaconManager = beaconManager;
            _logger = logger;
            _beaconOptions = new List<BeaconOption>
            {
                new BeaconOption(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 65438, 43487),2,-77, new Point(1, 1)),
                //new BeaconOption(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 56954, 34501),2,-77, new Point(1, 1)),
                //new BeaconOption(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 48943, 20570,2, -77,new Point(1, 1)),
                //new BeaconOption(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 35144, 19824,2, -77,new Point(1, 1.3))
            };
            _sphereFactory = new SphereFactory(Algoritm.CalculateDistance, _beaconOptions); // TODO: Можно зарегистрировать в DI
        }


        public ObservableCollection<SphereDto> Spheres { get; } = new ObservableCollection<SphereDto>();
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

            var whiteListBeaconsId = _beaconOptions.Select(b => b.BeaconId).ToList();
            _scanSub = _beaconManager
                .WhenBeaconRanged(scanRegion, BleScanType.LowLatency)
                .ManagedScanFlow(whiteListBeaconsId, TimeSpan.FromSeconds(1), _sphereFactory)
                //Аналитика
                .Do(sphere =>
                {
                })
                //Обработка
                .ObserveOnIf(_scheduler)
                .Synchronize(Spheres)
                .Subscribe(spheres =>
                 {
                     foreach (var sphere in spheres)
                     {
                         var managed = Spheres.FirstOrDefault(x => x.Sphere.BeaconId.Equals(sphere.BeaconId));
                         if (managed == null)
                         {
                             managed = new SphereDto(sphere);
                             Spheres.Add(managed);
                         }
                         managed.LastSeen = DateTimeOffset.UtcNow;
                         managed.Analitic = sphere.RangeList.Select(r=>r.ToString()).Aggregate((r1, r2) => $"{r1}, {r2}");
                         managed.Center = sphere.Center;
                         managed.Radius = sphere.Radius;
                         
                         //_logger.LogInformation($"{sphere.Beacon.Analitic}");
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
