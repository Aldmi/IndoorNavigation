using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using ApplicationCore.Domain;
using ApplicationCore.Domain.CheckPointModel.Trilateration.Spheres;
using ApplicationCore.Domain.DistanceService;
using ApplicationCore.Domain.DistanceService.Model;
using ApplicationCore.Domain.Options;
using ApplicationCore.Shared;
using ApplicationCore.Shared.Filters.Kalman;
using Libs.Beacons;
using Libs.Beacons.Flows;
using Libs.Beacons.Models;
using Libs.BluetoothLE;
using Libs.Excel;
using Microsoft.Extensions.Logging;
using Shiny;
using UseCase.Trilateration.Flow;
using Point = ApplicationCore.Shared.Models.Point;

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
        //private readonly SphereFactory _sphereFactory;
        
        
        public ManagedScan(
            IBeaconRangingManager beaconManager,
            IExcelAnalitic excelAnalitic,
            ILogger<ManagedScan> logger)
        {
            _beaconManager = beaconManager;
            _excelAnalitic = excelAnalitic;
            _logger = logger;
            _beaconOptions = new List<BeaconOption>
            {
                new BeaconOption(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 65438, 43487),2,-59, new Point(1, 1)),
                new BeaconOption(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 56954, 34501),2,-59, new Point(1, 1)),
                new BeaconOption(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 35144, 19824),2,-59, new Point(1, 1)),
               // new BeaconOption(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 48943, 20570),2,-77, new Point(1, 1)),
            };
            //_sphereFactory = new SphereFactory(Algoritms.CalculateDistance, _beaconOptions); // TODO: Можно зарегистрировать в DI
        }


        public ObservableCollection<BeaconDistanceStatisticDto> Statistic { get; } = new ObservableCollection<BeaconDistanceStatisticDto>();
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
                        .Synchronize(Statistic)
                        .Subscribe(_ =>
                        {
                            var maxAge = DateTimeOffset.UtcNow.Subtract(value.Value);
                            var tmp = Statistic.Where(x => x.LastSeen < maxAge).ToList();
                            foreach (var sphere in tmp)
                                Statistic.Remove(sphere);
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
            Statistic.Clear();

            // restart clear if applicable
            ClearTime = ClearTime;

            var whiteList = _beaconOptions.Select(b => b.BeaconId).ToList();
            var observableListStatistic = _beaconManager
                .WhenBeaconRanged(scanRegion, BleScanType.LowLatency)
                //Проходят только значения из списка
               // .WhenWhiteList(whiteList)
                .Beacon2BeaconDistance(
                    TimeSpan.FromSeconds(1.0),
                    0,
                    new Kalman1DFilterWrapper(1.5, 10.0, 2.0, TimeSpan.FromSeconds(5)),
                    10.0)
                .Publish()
                .RefCount();
            
            _scanSub = observableListStatistic
                //Обработка
                .ObserveOnIf(_scheduler)
                .Synchronize(Statistic)
                .Subscribe(beaconStat =>
                {
                    foreach (var stat in beaconStat)
                    {
                        var managed = Statistic.FirstOrDefault(x => x.Statistic.BeaconId.Equals(stat.BeaconId));
                        if (managed == null)
                        {
                            var statistic = new BeaconDistanceStatistic(stat.BeaconId, null, stat.Distance);
                            managed = new BeaconDistanceStatisticDto(statistic);
                            Statistic.Add(managed);
                        }
                        managed.LastSeen = DateTimeOffset.UtcNow;
                        //managed.DistanceList = stat.DistanceList.Select(r => r.ToString("F1")).Aggregate((s1, s2) => $"{s1} / {s2}");
                        managed.DistanceResult = stat.Distance.ToString("F1");
                    }
                    //TODO: можно вычислять местоположение. 
                    // var location= Trilateration.CalcLocation(spheres);
                 }, exception =>
                {
                    _logger?.LogError(exception, "Ошибка сканирования");
                });


            // _writeAnaliticSub = observableListStatistic
            //     .Buffer(10)
            //     .Subscribe(async statisticList =>
            //     {
            //         var csvHeader = BeaconDistanceStatisticCsv.CsvHeader;
            //         var csvLines = statisticList
            //             .SelectMany(list =>list.Select(stat=>BeaconDistanceStatisticCsv.Create(new BeaconDistanceStatistic(stat.BeaconId, null, stat.Distance), ExpectedRange4Analitic)))
            //             .Select(statistic => statistic.Convert2CsvFormat())
            //             .ToArray();
            //         await _excelAnalitic.Write2CsvDoc("BeaconDistance.txt", csvHeader, csvLines, _firstStart);
            //         _firstStart = false;
            //         Debug.WriteLine(ExpectedRange4Analitic);//DEBUG
            //     });
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
            Statistic.Clear();
        }
    }
}
