using System.Collections.Generic;
using System.Linq;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.Distance.Handlers
{
    public class BeaconDistanceAverageHandler: IBeaconDistanceHandler
    {
        public double Invoke(BeaconId id, IEnumerable<double> distances)
        {
            return distances.Average();
        }
    }
}