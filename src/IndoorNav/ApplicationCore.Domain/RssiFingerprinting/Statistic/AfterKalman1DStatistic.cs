using System;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.RssiFingerprinting.Statistic
{
    public class AfterKalman1DStatistic
    {
        public AfterKalman1DStatistic(BeaconId beaconId, double rssiBefore, double rssiAfter, double distanceBefore, double distanceAfter, DateTimeOffset lastSeen)
        {
            BeaconId = beaconId;
            RssiBefore = rssiBefore;
            RssiAfter = rssiAfter;
            DistanceBefore = distanceBefore;
            DistanceAfter = distanceAfter;
            LastSeen = lastSeen;
        }
        
        public BeaconId BeaconId { get; }
        public double RssiBefore { get; }
        public double RssiAfter { get; }
        public double DistanceBefore { get; }
        public double DistanceAfter { get; }
        public DateTimeOffset LastSeen { get; }
    }
}