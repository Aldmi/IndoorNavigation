using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.DistanceService.Handlers
{
    public class BeaconDistanceAverageHandler: IBeaconDistanceHandler
    {
        public double Invoke(BeaconId id, IEnumerable<double> distances)
        {
            //Debug.WriteLine(String.Join("  ", distances.Select(d => d.ToString("F"))));
            return distances.Average();
        }
    }
}