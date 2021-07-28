using System;

namespace Libs.Beacons.Models
{
    public class ExtBeacon
    {
        public ExtBeacon(Beacon beacon)
        {
            Beacon = beacon;
            LastSeen=DateTimeOffset.UtcNow;
        }

        public Beacon Beacon { get; }
        public DateTimeOffset LastSeen { get;} 

        public override string ToString() => $"{Beacon} - {LastSeen:hh:mm:ss}";
    }
}