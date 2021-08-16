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
        
        
        public ObservableCollection<MovingDto> Movings { get; } = new ObservableCollection<MovingDto>();
        public BeaconRegion? ScanningRegion { get; private set; }
        public bool IsScanning => ScanningRegion != null;
        
        
        
        public void Start(IScheduler? scheduler = null)
        {
            if (IsScanning)
                throw new ArgumentException("A beacon scan is already running");
            
            _scheduler = scheduler;
            
            Movings.Clear();
            
            //Загрузить граф если граф пуст.
            _graph ??= _graphRepository.GetGraph();
            ScanningRegion ??= new BeaconRegion("Graph root", _graph.RootId.Uuid);

            var observableListMovings = _beaconManager
                .WhenBeaconRanged(ScanningRegion, BleScanType.LowLatency)
                .ManagedScanDiscreteStepsFlow(
                    TimeSpan.FromSeconds(1),
                    -77, //TODO: брать из протокола.
                    _graph.CalculateMove,
                    _logger);
                //Выдавать только первый найденный CheckPoint и затем только готовые отрезки.
                //.Where(moving =>moving.MovingEvent == MovingEvent.InitSegment || moving.MovingEvent == MovingEvent.CompleteSegment);
            
            _scanSub = observableListMovings
                //Обработка
                .ObserveOnIf(_scheduler)
                .Synchronize(Movings)
                .Subscribe(moving =>
                {
                    
                    Movings.Add(new MovingDto(moving.Start, moving.End, moving.MovingEvent));

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