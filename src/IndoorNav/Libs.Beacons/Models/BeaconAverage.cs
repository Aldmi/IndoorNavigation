using System;

namespace Libs.Beacons.Models
{
    /// <summary>
    /// маяк с усредненным значением Rssi.
    /// </summary>
    public class BeaconAverage : IEquatable<BeaconAverage>
    {
        public BeaconAverage(BeaconId id, double rssi, int txPower)
        {
            Id = id;
            Rssi = rssi;
            TxPower = txPower;
        }


        public BeaconId Id { get;  }
        public double Rssi { get; }
        public int TxPower { get; }
        
        
        public override string ToString() => $"[Beacon: {Id}, Rssi= {Rssi}, TxPower= {TxPower}]";

        
        public bool Equals(BeaconAverage? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id);
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
            return HashCode.Combine(Id, Rssi, TxPower);
        }
    }
}