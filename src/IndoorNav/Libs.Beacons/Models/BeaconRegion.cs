using System;

namespace Libs.Beacons.Models
{
    public class BeaconRegion : IEquatable<BeaconRegion>
    {
        public BeaconRegion(string identifier, Guid uuid, ushort? major = null, ushort? minor = null)
        {
            Identifier = identifier;
            Uuid = uuid;

            if (major != null)
            {
                if (major < 1)
                    throw new ArgumentException("Invalid Major Value");

                Major = major;
            }

            if (minor != null)
            {
                if (major == null)
                    throw new ArgumentException("You must provide a major value if you are setting minor");

                if (minor < 1)
                    throw new ArgumentException("Invalid Minor Value");

                Minor = minor;
            }
        }


        public string Identifier { get; }
        public Guid Uuid { get; }
        public ushort? Major { get; }
        public ushort? Minor { get; }
        public bool NotifyOnEntry { get; set; } = true;
        public bool NotifyOnExit { get; set; } = true;


        public override string ToString() => $"[Identifier: {Identifier} - UUID: {Uuid} - Major: {Major ?? 0} - Minor: {Minor ?? 0}]";
        public bool Equals(BeaconRegion other) => (Identifier, Uuid, Major, Minor).Equals((other?.Identifier, other?.Uuid, other?.Major, other?.Minor));
        public static bool operator ==(BeaconRegion? left, BeaconRegion? right) => Equals(left, right);
        public static bool operator !=(BeaconRegion left, BeaconRegion right) => !Equals(left, right);
        public override bool Equals(object obj) => obj is BeaconRegion region && Equals(region);
        public override int GetHashCode() => (Identifier, Uuid, Major, Minor).GetHashCode();
    }
}