using System;
using System.Collections.ObjectModel;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using ApplicationCore.Shared.Filters.Kalman;
using ApplicationCore.Shared.Services;
using Libs.Beacons;
using Libs.Beacons.Models;
using Libs.BluetoothLE;
using Microsoft.Extensions.Logging;
using Shiny;
using UseCase.RssiFingerprinting.Flows;
using UseCase.RssiFingerprinting.Infrastructure.DataAccess;

namespace UseCase.RssiFingerprinting.Managed
{
    public class ManagedRssiFingerprinting : IDisposable
    {
        private readonly IBeaconRangingManager _beaconManager;
        private readonly ITotalFingerprintRepository _totalFingerprintRepository;
        private IScheduler? _scheduler;
        private readonly ILogger<ManagedRssiFingerprinting>? _logger;
        private IDisposable? _scanSub;
        
        
        public ManagedRssiFingerprinting(
            IBeaconRangingManager beaconManager,
            ITotalFingerprintRepository totalFingerprintRepository,
            ILogger<ManagedRssiFingerprinting> logger)
        {
            _beaconManager = beaconManager;
            _totalFingerprintRepository = totalFingerprintRepository;
            _logger = logger;
        }
        
        
        public BeaconRegion? ScanningRegion { get; private set; }
        public bool IsScanning => ScanningRegion != null;
        public ObservableCollection<TotalFingerprintDto> TotalFingerprints { get; } = new ObservableCollection<TotalFingerprintDto>();
        
        
        
        public void Start(BeaconRegion scanRegion, IScheduler? scheduler = null)
        {
            if (IsScanning)
                throw new ArgumentException("A TotalFingerprint scan is already running");
            
            _scheduler = scheduler;
            ScanningRegion = scanRegion;
            TotalFingerprints.Clear();

            var totalList = _totalFingerprintRepository.GetTotalFingerprintList();
            
            var observableListStatistic = _beaconManager
                .WhenBeaconRanged(scanRegion, BleScanType.LowLatency)
                .Beacon2TotalFingerprint(
                    TimeSpan.FromSeconds(1.0),
                    Kalman1DFilterWrapper.GetLargeMeasurementErrorFilterWrapper, 
                    totalList,
                    10.0)
                .Publish()
                .RefCount();
            
            _scanSub = observableListStatistic
                //Обработка
                .ObserveOnIf(_scheduler)
                .Synchronize(TotalFingerprints)
                .Subscribe(tfRes =>
                {
                    if (tfRes.IsSuccess)
                    {
                        var dto = new TotalFingerprintDto(tfRes.Value.RoomCoordinate);
                        TotalFingerprints.Add(dto);
                    }
                    else
                    {
                        _logger?.LogError($"Result is failure '{tfRes.Error}'");
                    }
                    
                }, exception =>
                {
                    _logger?.LogError(exception, "Exception при сканировании");
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
            TotalFingerprints.Clear();
        }
    }
}