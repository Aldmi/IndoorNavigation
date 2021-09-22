using System;

namespace Libs.Beacons.Models
{
    public class Beacon : IEquatable<Beacon>
    {
        public Beacon(Guid uuid, ushort major, ushort minor, Proximity proximity, int rssi, double accuracy, int txPower)
        {
            Id = new BeaconId(uuid, major, minor);
            Uuid = uuid;
            Major = major;
            Minor = minor;
            Proximity = proximity;
            Rssi = rssi;
            Accuracy = accuracy;
            TxPower = txPower;
        }
        
        
        public Beacon(BeaconId id, int rssi, int txPower)
        {
            Id = id;
            Uuid = Id.Uuid; //TODO: переделать только под Id
            Major = Id.Major;
            Minor = Id.Minor;
            Rssi = rssi;
            TxPower = txPower;
        }


        public BeaconId Id { get;  }
        public Guid Uuid { get; }
        public ushort Minor { get; }
        public ushort Major { get; }
        public Proximity Proximity { get; }
        public int Rssi { get; }
        public double Accuracy { get; }
        public int TxPower { get; }


        public override string ToString() => $"[Beacon: Uuid={Uuid}, Major={Major}, Minor={Minor}, Rssi= {Rssi}]";
        public bool Equals(Beacon other) => (Uuid, Major, Minor) == (other.Uuid, other.Major, other.Minor);
        public static bool operator ==(Beacon left, Beacon right) => Equals(left, right);
        public static bool operator !=(Beacon left, Beacon right) => !Equals(left, right);
        public override bool Equals(object obj) => obj is Beacon beacon && Equals(beacon);
        public override int GetHashCode() => (Uuid, Major, Minor).GetHashCode();
    }
}