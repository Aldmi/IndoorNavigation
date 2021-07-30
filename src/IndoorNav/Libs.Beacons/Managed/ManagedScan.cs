using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Android.Graphics;
using Java.Util;
using Libs.Beacons.Managed.Flows.FilterFlow;
using Libs.Beacons.Managed.Flows.TrilaterationFlow;
using Libs.Beacons.Managed.Options;
using Libs.Beacons.Models;
using Libs.BluetoothLE;
using Microsoft.Extensions.Logging;
using Shiny;
using Observable = System.Reactive.Linq.Observable;
using Point = Libs.Beacons.Managed.Flows.TrilaterationFlow.Point;

namespace Libs.Beacons.Managed
{
    public class ManagedScan : IDisposable
    {
        private readonly IBeaconRangingManager _beaconManager;
        private readonly ILogger? _logger;
        private IScheduler? _scheduler;
        private IDisposable? _clearSub;
        private IDisposable? _scanSub;
        private readonly IEnumerable<BeaconOption> _beaconOptions; //TODO: внедрять сервис репозитория, загружать настройки из БД.
        
        
        public ManagedScan(IBeaconRangingManager beaconManager, ILogger<ManagedScan> logger)
        {
            _beaconManager = beaconManager;
            _logger = logger;
            _beaconOptions = new List<BeaconOption>
            {
                new BeaconOption(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 65438, 43487,2,-77, new Point(1, 1)),
                //new BeaconOption(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 56954, 34501,2,-77, new Point(1, 1)),
                //new BeaconOption(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 48943, 20570,2, -77,new Point(1, 1)),
                //new BeaconOption(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 35144, 19824,2, -77,new Point(1, 1.3))
            };
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
                //Проходят только значения из списка
                .WhenWhiteList(_beaconOptions)
                //Фильтр
                .AverageFilter(
                    TimeSpan.FromSeconds(1),
                    rssiList => (int) Math.Round(rssiList.Average(r => r))
                    )
                //Создание сфер
                .CreateSphere(_beaconOptions)
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
                        
                        _logger.LogInformation($"{sphere.Beacon.Rssi}");
                    }
            
                    //TODO: можно вычислять местоположение. 
                  // var location= Trilateration.CalcLocation(spheres);
                }, exception =>
                {
                    _logger?.LogError(exception, "Ошибка сканирования");
                });
            
            
            // _scanSub = _beaconManager
            //     .WhenBeaconRanged(scanRegion, BleScanType.LowLatency)
            //     .Buffer(TimeSpan.FromSeconds(1))   //2
            //     .ObserveOnIf(_scheduler)
            //     .Synchronize(Spheres)
            //     .Subscribe(beacons =>
            //     {
            //         foreach (var beacon in beacons)
            //         {
            //             Debug.WriteLine($"beacon!!!!  Proximity={beacon.Proximity:G}   Rssi={beacon.Rssi}   Accuracy={beacon.Accuracy}   {DateTime.UtcNow:T} >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
            //         }
            //     });
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
