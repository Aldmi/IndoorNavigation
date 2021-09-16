using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ApplicationCore.Domain.CheckPointModel;
using ApplicationCore.Domain.DistanceService;
using ApplicationCore.Domain.DistanceService.Handlers;
using ApplicationCore.Domain.MovingService;
using ApplicationCore.Domain.MovingService.Model;
using ApplicationCore.Domain.RouteTrackingService;
using ApplicationCore.Domain.RouteTrackingService.Model;
using Libs.Beacons;
using Libs.Beacons.Models;
using Libs.BluetoothLE;
using Libs.Excel;
using Microsoft.Extensions.Logging;
using Shiny;


namespace UseCase.DiscreteSteps.Managed
{
    public class ManagedGraph : IDisposable
    {
        private readonly IBeaconRangingManager _beaconManager;
        private readonly ICheckPointGraphRepository _graphRepository;
        private readonly IBeaconDistanceHandler _beaconDistanceHandler;
        private readonly IExcelAnalitic _excelAnalitic;
        
        private IGraphMovingCalculator _graphMovingCalculator;
        private IRouteBuilder _routeBuilder;
        private IRouteTracker _routeTracker;
        
        private readonly ILogger? _logger;
        private IDisposable? _scanSub;
        private IDisposable? _writeAnaliticSub;
        private IDisposable? _trackingSub;
        private readonly Subject<MovingDto> _lastMovingSubj = new Subject<MovingDto>();
        
        private IObservable<Moving> _observableListMovings;

        
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
            
            Init();//DEBUG (вызывать отдельно, после создания объекта ManagedGraph)
        }
        
        
        public ObservableCollection<MovingDto> Movings { get; } = new ObservableCollection<MovingDto>();
        public ObservableCollection<TrackingDto> Trackings { get; } = new ObservableCollection<TrackingDto>();
        public BeaconRegion? ScanningRegion { get; private set; }
        public bool IsScanning => ScanningRegion != null;
        public Route? Route { get; private set; }
        public IObservable<MovingDto> LastMoving => _lastMovingSubj.AsObservable();



        private void Init()
        {
            //Загрузить граф если граф пуст.
            _graphMovingCalculator = new GraphMovingCalculator(_graphRepository.GetSharedUuid(), _graphRepository.GetGraph()); //TODO: сами сервисы внедрять через фабрику Func<>();
            _routeBuilder = new RouteBuilder(_graphRepository.GetGraph());
            ScanningRegion = new BeaconRegion("Graph root", _graphMovingCalculator.SharedUuid);
            
           _observableListMovings= _beaconManager
                .WhenBeaconRanged(ScanningRegion, BleScanType.LowLatency)
                .Beacon2BeaconDistance(
                    TimeSpan.FromSeconds(0.6),
                    -59, 
                    _beaconDistanceHandler.Invoke)
                 //Определить перемещение в графе движения, используя функцию calculateMove.
                .Select(listDistance=> _graphMovingCalculator.CalculateMove(listDistance))
                //Выдавать только первый найденный CheckPoint и затем только готовые отрезки.
                //.Where(moving =>moving.MovingEvent == MovingEvent.InitSegment || moving.MovingEvent == MovingEvent.CompleteSegment);
                .Publish()
                .RefCount();
        }

        
        
        public void Start(IScheduler? scheduler = null)
        {
            if (IsScanning)
                throw new ArgumentException("A beacon scan is already running");

            Movings.Clear();

            
            _scanSub = _observableListMovings
                //Обработка
                .ObserveOnIf(scheduler)  // Запустить обратные вызовы наблюдателя в указанном планировщике. (В данном случае передается поток UI.)
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

            //AnaliticRec();
        }


        /// <summary>
        /// Построить маршрут.
        /// </summary>
        /// <param name="routeName"></param>
        /// <param name="endCh"></param>
        /// <exception cref="Exception"></exception>
        public void BuildRoute(string routeName, CheckPointBase endCh)
        {
           var startCh = _graphMovingCalculator.CurrentCheckPoint;
           if (startCh == null)
           {
               throw new Exception("Стартовая точка не установлена"); //TODO: создать custom exception
           }
           Route = _routeBuilder.Build(routeName, startCh, endCh);
        }

        
        public void StartTrackingRoute(IScheduler? scheduler = null)
        {
            Trackings.Clear();
            _routeTracker.SetRoute(Route);
            _trackingSub = _observableListMovings
                .ObserveOnIf(scheduler)
                .Synchronize(Trackings)
                .Select(moving => _routeTracker.Control(moving))
                .Subscribe(tracking =>
                {
                    var dto = new TrackingDto();
                    Trackings.Add(dto);
                }, exception =>
                {
                    _logger?.LogError(exception, "Ошибка отслеживания маршрута");
                });
        }
        
        
        private bool _firstRecAnalitic;
        private void AnaliticRec()
        {
            _firstRecAnalitic = true;
            _writeAnaliticSub = _observableListMovings
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
                    await _excelAnalitic.Write2CsvDoc("DiscreteStepAnalitic.txt", csvHeader, csvLines, _firstRecAnalitic);
                    _firstRecAnalitic = false;
                    //Debug.WriteLine(csvLines.Aggregate((s1,s2)=> s1+" "+s2));//DEBUG
                });
        }
        
        
        
        public void StopScan()
        {
            _scanSub?.Dispose();
            _writeAnaliticSub?.Dispose();
            _graphMovingCalculator?.Reset();
            ScanningRegion = null;
        }
        
        
        public void StopTrackingRoute()
        {
            _trackingSub?.Dispose();
        }


        public void Dispose()
        {
            StopScan();
            _lastMovingSubj.Dispose();
            Movings.Clear();
        }
    }
}