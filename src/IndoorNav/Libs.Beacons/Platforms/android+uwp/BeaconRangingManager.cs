using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Libs.Beacons.Models;
using Libs.BluetoothLE;
using Shiny;

namespace Libs.Beacons.Platforms
{
    public class BeaconRangingManager : IBeaconRangingManager
    {
        private readonly IBleManager _bleManager;
        private IObservable<Beacon>? _beaconScanner;


        public BeaconRangingManager(IBleManager centralManager)
            => _bleManager = centralManager;


        public Task<AccessState> RequestAccess() => _bleManager.RequestAccess().ToTask();

        public IObservable<Beacon> WhenBeaconRanged(BeaconRegion? region, BleScanType scanType) => _beaconScanner ??=
            _bleManager
                .ScanForBeacons(scanType)
                .Where(beacon => region == null || region.IsBeaconInRegion(beacon))
                .Publish()
                .RefCount();
    }
}