using System.Threading.Tasks;
using Libs.Beacons.Models;

namespace Libs.Beacons
{
    public interface IBeaconMonitorDelegate
    {
        Task OnStatusChanged(BeaconRegionState newStatus, BeaconRegion region);
    }
}
