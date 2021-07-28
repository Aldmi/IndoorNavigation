using System;
using Libs.Beacons.Models;

namespace Libs.Beacons.Platforms
{
    public class BeaconRegionStatus
    {
        public BeaconRegionStatus(BeaconRegion region) => Region = region;
        public BeaconRegion Region { get; }
        public bool? IsInRange { get; set; }
        public DateTimeOffset LastPing { get; set; }
    }
}