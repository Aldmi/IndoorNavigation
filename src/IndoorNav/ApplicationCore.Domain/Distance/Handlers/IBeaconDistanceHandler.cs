using System.Collections.Generic;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.Distance.Handlers
{
    public interface IBeaconDistanceHandler
    {
        double Invoke(BeaconId id, IEnumerable<double> distances);
    }
}