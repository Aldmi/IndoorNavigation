using System.Collections.Generic;
using Libs.Beacons.Models;

namespace Test.Beacons.Domain.Test.DistanceService
{
    public class DistanceListByBeacon
    {
        public BeaconId Id { get; set; }
        public List<double> Distances { get; set; }
    }
}