using System;

namespace Libs.Beacons.Models
{
    /// <summary>
    /// маяк с усредненным значением Rssi.
    /// </summary>
    public class BeaconAverage : IEquatable<BeaconAverage>
    {
        public BeaconId BeaconId { get;  }
        public double Rssi { get; }
        public int TxPower { get; }
        public DateTimeOffset LastSeen { get; }
        
        
        public BeaconAverage(BeaconId beaconId, double rssi, int txPower)
        {
            BeaconId = beaconId;
            Rssi = rssi;
            TxPower = txPower;
            LastSeen = DateTimeOffset.UtcNow;
        }

        public BeaconAverage CreateWithNewRssi(double rssi) => new BeaconAverage(BeaconId, rssi, TxPower);
        
        public bool Equals(BeaconAverage? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return BeaconId.Equals(other.BeaconId);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BeaconAverage) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BeaconId, Rssi, TxPower);
        }
        
        public override string ToString() => $"[Beacon: {BeaconId}, Rssi= {Rssi}, TxPower= {TxPower}]";
    }
}