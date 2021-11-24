using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using ApplicationCore.Shared.Filters.Kalman;
using Libs.Beacons;
using Libs.Beacons.Models;
using Libs.BluetoothLE;
using Libs.Excel;
using Microsoft.Extensions.Logging;
using Shiny;
using UseCase.RssiFingerprinting.Flows;

namespace UseCase.RssiFingerprinting.ManagedKalmanStatistic
{
    public class ManagedKalman1D : IDisposable
    {
        private readonly IBeaconRangingManager _beaconManager;
        private readonly IExcelAnalitic _excelAnalitic;
        private IScheduler? _scheduler;
        private readonly ILogger? _logger;
        private IDisposable? _scanSub;
        private IDisposable? _writeAnaliticSub;

        
        public ManagedKalman1D(
            IBeaconRangingManager beaconManager,
            IExcelAnalitic excelAnalitic,
            ILogger<ManagedKalman1D>? logger)
        {
            _beaconManager = beaconManager;
            _excelAnalitic = excelAnalitic;
            _logger = logger;
            
            ScanningRegion = new BeaconRegion("Graph root", Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"));
        }
        
        
        public BeaconRegion? ScanningRegion { get; private set; }
        public bool IsScanning => ScanningRegion != null;
        public ObservableCollection<AfterKalman1DStatisticDto> Statistics { get; } = new ObservableCollection<AfterKalman1DStatisticDto>();
        public int ExpectedRange { get; set; }
        
        
        
        public void Start(IScheduler? scheduler = null)
        {
            if (IsScanning)
                throw new ArgumentException("A TotalFingerprint scan is already running");
            
            _scheduler = scheduler;
            Statistics.Clear();
            
            var observableListStatistic = _beaconManager
                .WhenBeaconRanged(ScanningRegion, BleScanType.LowLatency)
                .BeaconKalman1DStatistic(
                    TimeSpan.FromSeconds(1.0),
                    Kalman1DFilterWrapper.GetLargeMeasurementErrorFilterWrapper,
                    10.0)
                .Publish()
                .RefCount();
            
            _scanSub = observableListStatistic
                //Обработка
                .ObserveOnIf(_scheduler)
                .Synchronize(Statistics)
                .Subscribe(afterKalman1DStatistics =>
                {
                    foreach (var stat in afterKalman1DStatistics)
                    {
                        var managed = Statistics.FirstOrDefault(dto => dto.AfterKalman1DStatistic.BeaconId.Equals(stat.BeaconId));
                        if (managed == null)
                        {
                            managed = new AfterKalman1DStatisticDto(stat);
                            Statistics.Add(managed);
                        }
                        managed.LastSeen = DateTimeOffset.UtcNow;
                        managed.RssiBefore = stat.RssiBefore;
                        managed.RssiAfter = stat.RssiAfter;
                        managed.DistanceBefore = stat.DistanceBefore;
                        managed.DistanceAfter = stat.DistanceAfter;
                    }
                }, exception =>
                {
                    _logger?.LogError(exception, "Exception при сканировании");
                });
        }
        
        
        public void StopScan()
        {
            _writeAnaliticSub?.Dispose();
            _scanSub?.Dispose();
            _scheduler = null;
            ScanningRegion = null;
        }
        
        public void Dispose()
        {
            StopScan();
            Statistics.Clear();
        }
    }
}