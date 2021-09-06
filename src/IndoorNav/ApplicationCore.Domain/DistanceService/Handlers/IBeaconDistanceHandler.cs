using System.Collections.Generic;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.DistanceService.Handlers
{
    public interface IBeaconDistanceHandler
    {
        double Invoke(BeaconId id, IEnumerable<double> distances);
    }
}