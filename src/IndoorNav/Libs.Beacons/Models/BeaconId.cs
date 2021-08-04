using System;

namespace Libs.Beacons.Models
{
    public class BeaconId : IEquatable<BeaconId>
    {
        public BeaconId(Guid uuid, ushort major, ushort minor)
        {
            Uuid = uuid;
            Major = major;
            Minor = minor;
        }

        public Guid Uuid { get; }
        public ushort Major { get; }
        public ushort Minor { get; }


        public static bool operator ==(BeaconId left, BeaconId right) => Equals(left, right);
        public static bool operator !=(BeaconId left, BeaconId right) => !Equals(left, right);
        
        public bool Equals(BeaconId? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Uuid.Equals(other.Uuid) && Major == other.Major && Minor == other.Minor;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BeaconId) obj);
        }
        
        public override int GetHashCode() => (Uuid, Major, Minor).GetHashCode();
        //public override int GetHashCode20()=> HashCode.Combine(Uuid, Major, Minor); //TODO: не рабоатет в тестах
        
        public override string ToString() => $"{Uuid}. {Major}/{Minor}";
    }
}