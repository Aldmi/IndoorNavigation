using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ApplicationCore.Domain.DiscreteSteps;
using ApplicationCore.Domain.Distance;
using ApplicationCore.Domain.Distance.Handlers;
using ApplicationCore.Domain.Navigation;
using ApplicationCore.Domain.Navigation.Statistic;
using ApplicationCore.Domain.Trilateration.Spheres;
using Libs.Beacons;
using Libs.Beacons.Models;
using Libs.BluetoothLE;
using Libs.Excel;
using Microsoft.Extensions.Logging;
using Shiny;
using UseCase.DiscreteSteps.Flow;

namespace UseCase.DiscreteSteps.Managed
{
    public class ManagedGraph : IDisposable
    {
        private readonly IBeaconRangingManager _beaconManager;
        private readonly ICheckPointGraphRepository _graphRepository;
        private readonly IBeaconDistanceHandler _beaconDistanceHandler;
        private readonly IExcelAnalitic _excelAnalitic;
        private GraphMovingCalculator? _graphService;
        private readonly ILogger? _logger;
        private IScheduler? _scheduler;
        private IDisposable? _scanSub;
        private IDisposable? _writeAnaliticSub;
        private readonly Subject<MovingDto> _lastMovingSubj = new Subject<MovingDto>();


        public ManagedGraph(
            IBeaconRangingManager beaconManager,
            ICheckPointGraphRepository graphRepository,
            IBeaconDistanceHandler beaconDistanceHandler,
            IExcelAnalitic excelAnalitic,
            ILogger<ManagedGraph> logger)
        {
            _beaconManager = beaconManager;
            _graphRepository = graphRepository;
            _beaconDistanceHandler = beaconDistanceHandler;
            _excelAnalitic = excelAnalitic;
            _logger = logger;
        }
        
        
        public ObservableCollection<MovingDto> Movings { get; } = new ObservableCollection<MovingDto>();
        public BeaconRegion? ScanningRegion { get; private set; }
        public bool IsScanning => ScanningRegion != null;
        public IObservable<MovingDto> LastMoving => _lastMovingSubj.AsObservable();


        private bool _firstStart;
        public void Start(IScheduler? scheduler = null)
        {
            if (IsScanning)
                throw new ArgumentException("A beacon scan is already running");
            
            _firstStart = true;
            _scheduler = scheduler;
            Movings.Clear();
            
            //Загрузить граф если граф пуст.
            _graphService ??= new GraphMovingCalculator(_graphRepository.GetGraph());
            ScanningRegion ??= new BeaconRegion("Graph root", _graphService.SharedUuid);

            var observableListMovings = _beaconManager
                .WhenBeaconRanged(ScanningRegion, BleScanType.LowLatency)
                .ManagedScanDiscreteStepsFlow(
                    TimeSpan.FromSeconds(1),
                    -59, //TODO: брать из протокола.
                    _beaconDistanceHandler.Invoke,
                    _graphService.CalculateMove,
                    _logger)
                //Выдавать только первый найденный CheckPoint и затем только готовые отрезки.
                //.Where(moving =>moving.MovingEvent == MovingEvent.InitSegment || moving.MovingEvent == MovingEvent.CompleteSegment);
                .Publish()
                .RefCount();
            
            _scanSub = observableListMovings
                //Обработка
                .ObserveOnIf(_scheduler)  // Запустить обратные вызовы наблюдателя в указанном планировщике. (В данном случае передается поток UI.)
                .Synchronize(Movings)
                .Subscribe(moving =>
                {
                    var dto = new MovingDto(moving.Start, moving.End, moving.MovingEvent);
                    _lastMovingSubj.OnNext(dto);
                    Movings.Add(dto);
                }, exception =>
                {
                    _logger?.LogError(exception, "Ошибка сканирования");
                });
            
            
            _writeAnaliticSub = observableListMovings
                .Buffer(5)
                .Subscribe(async listMovings =>
                {
                    var csvHeader = MovingCsvStatistic.CsvHeader;
                    var csvLines = listMovings
                        .Select(moving =>
                        {
                            var statistic = MovingCsvStatistic.Create(moving);
                            return statistic.Convert2CsvFormat();
                        })
                        .ToArray();
                    await _excelAnalitic.Write2CsvDoc("DiscreteStepAnalitic.txt", csvHeader, csvLines, _firstStart);
                    _firstStart = false;
                    //Debug.WriteLine(csvLines.Aggregate((s1,s2)=> s1+" "+s2));//DEBUG
                });
            
        }
        
        
        public void Stop()
        {
            _scanSub?.Dispose();
            _writeAnaliticSub?.Dispose();
            _graphService?.Reset();
            _scheduler = null;
            ScanningRegion = null;
        }


        public void Dispose()
        {
            Stop();
            _lastMovingSubj.Dispose();
            Movings.Clear();
        }
    }
}