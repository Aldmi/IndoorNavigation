using System;
using System.Collections.ObjectModel;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using ApplicationCore.Domain.DiscreteSteps;
using Libs.Beacons;
using Libs.Beacons.Models;
using Libs.BluetoothLE;
using Microsoft.Extensions.Logging;
using Shiny;
using UseCase.DiscreteSteps.Flow;

namespace UseCase.DiscreteSteps.Managed
{
    public class ManagedGraph : IDisposable
    {
        private readonly IBeaconRangingManager _beaconManager;
        private readonly ICheckPointGraphRepository _graphRepository;
        private CheckPointGraph? _graph;
        private readonly ILogger? _logger;
        private IScheduler? _scheduler;
        private IDisposable? _scanSub;


        public ManagedGraph(IBeaconRangingManager beaconManager, ICheckPointGraphRepository graphRepository, ILogger<ManagedGraph> logger)
        {
            _beaconManager = beaconManager;
            _graphRepository = graphRepository;
            _logger = logger;
        }
        
        
        public ObservableCollection<Moving> Movings { get; } = new ObservableCollection<Moving>();
        public BeaconRegion? ScanningRegion { get; private set; }
        public bool IsScanning => ScanningRegion != null;
        
        
        
        public void Start(BeaconRegion scanRegion, IScheduler? scheduler = null)
        {
            if (IsScanning)
                throw new ArgumentException("A beacon scan is already running");
            
            _scheduler = scheduler;
            ScanningRegion = scanRegion;

            //Загрузить граф если граф пуст.
            _graph ??= _graphRepository.GetGraph();
            
            var observableListMovings= _beaconManager
                .WhenBeaconRanged(scanRegion, BleScanType.LowLatency)
                .ManagedScanDiscreteStepsFlow(
                    TimeSpan.FromSeconds(1),
                    -77,
                    _graph.CalculateMove,
                    _logger);
            
            _scanSub = observableListMovings
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
                .Synchronize(Movings)
                .Subscribe(moving =>
                {
                    // foreach (var sphere in spheres)
                    // foreach (var sphere in spheres)
                    // {
                    //     var managed = Spheres.FirstOrDefault(x => x.Sphere.BeaconId.Equals(sphere.BeaconId));
                    //     if (managed == null)
                    //     {
                    //         managed = new SphereDto(sphere);
                    //         Spheres.Add(managed);
                    //     }
                    //     managed.LastSeen = DateTimeOffset.UtcNow;
                    //     managed.Analitic = sphere.RangeList.Select(r=>r.ToString()).Aggregate((r1, r2) => $"{r1}, {r2}");
                    //     managed.Center = sphere.Center;
                    //     managed.Radius = sphere.Radius;
                    //      
                    //     //_logger.LogInformation($"{sphere.Beacon.Analitic}");
                    // }
                }, exception =>
                {
                    _logger?.LogError(exception, "Ошибка сканирования");
                });
            
        }
        
        
        
        public void Stop()
        {
            _scanSub?.Dispose();
            _scheduler = null;
            ScanningRegion = null;
        }


        public void Dispose()
        {
            Stop();
            Movings.Clear();
        }
    }
}