using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using ApplicationCore.Domain;
using ApplicationCore.Domain.Options;
using ApplicationCore.Domain.Shared;
using ApplicationCore.Domain.Trilateration.Spheres;
using Libs.Beacons;

using Libs.Beacons.Models;
using Libs.BluetoothLE;
using Libs.Excel;
using Microsoft.Extensions.Logging;
using Shiny;
using UseCase.Trilateration.Flow;
using Point = ApplicationCore.Domain.Shared.Point;

namespace UseCase.Trilateration.Managed
{
    public class ManagedScan : IDisposable
    {
        private readonly IBeaconRangingManager _beaconManager;
        private readonly IExcelAnalitic _excelAnalitic;
        private readonly ILogger? _logger;
        private IScheduler? _scheduler;
        private IDisposable? _clearSub;
        private IDisposable? _scanSub;
        private IDisposable? _writeAnaliticSub;
        private readonly IEnumerable<BeaconOption> _beaconOptions; //TODO: внедрять сервис репозитория, загружать настройки из БД.
        private readonly SphereFactory _sphereFactory;
        
        
        public ManagedScan(IBeaconRangingManager beaconManager, IExcelAnalitic excelAnalitic, ILogger<ManagedScan> logger)
        {
            _beaconManager = beaconManager;
            _excelAnalitic = excelAnalitic;
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
        public int ExpectedRange4Analitic { get; set; }
        


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

        private bool _firstStart;
        public void Start(BeaconRegion scanRegion, IScheduler? scheduler = null)
        {
            if (IsScanning)
                throw new ArgumentException("A beacon scan is already running");

            _firstStart = true;
            _scheduler = scheduler;
            ScanningRegion = scanRegion;
            Spheres.Clear();

            // restart clear if applicable
            ClearTime = ClearTime;

            var whiteListBeaconsId = _beaconOptions.Select(b => b.BeaconId).ToList();

            var observableListSphere = _beaconManager
                .WhenBeaconRanged(scanRegion, BleScanType.LowLatency)
                .ManagedScanFlow(whiteListBeaconsId, TimeSpan.FromSeconds(1), _sphereFactory);
            
            _scanSub = observableListSphere
                //Аналитика
                // .Do(async spheres =>
                // {
                //     var csvHeader = SphereStatistic.CsvHeader;
                //     var csvLines = spheres.Select(SphereStatistic.Create).Select(statistic => statistic.Convert2CsvFormat()).ToArray();
                //     await _excelAnalitic.Write2CsvDoc(csvHeader, csvLines, _firstStart);
                //     _firstStart = false;
                // })
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


            _writeAnaliticSub = observableListSphere
                .Buffer(5)
                .Subscribe(async spheres =>
                {
                    var csvHeader = SphereStatistic.CsvHeader;
                    var csvLines = spheres
                        .SelectMany(list =>list.Select(s=>SphereStatistic.Create(s, ExpectedRange4Analitic)))
                        .Select(statistic => statistic.Convert2CsvFormat())
                        .ToArray();
                    //await _excelAnalitic.Write2CsvDoc(csvHeader, csvLines, _firstStart);
                    _firstStart = false;
                    Debug.WriteLine(ExpectedRange4Analitic);//DEBUG
                });
        }


        public void Stop()
        {
            _clearSub?.Dispose();
            _writeAnaliticSub?.Dispose();
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
