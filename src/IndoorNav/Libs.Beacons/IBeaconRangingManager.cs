using System;
using System.Threading.Tasks;
using Libs.Beacons.Models;
using Libs.BluetoothLE;
using Shiny;

namespace Libs.Beacons
{
    public interface IBeaconRangingManager
    {
        /// <summary>
        /// Request necessary permissions to beacon scanning
        /// </summary>
        /// <returns></returns>
        Task<AccessState> RequestAccess();


        /// <summary>
        /// Engage the beacon ranging observable
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        IObservable<Beacon> WhenBeaconRanged(BeaconRegion? region, BleScanType scanType);
    }
}
