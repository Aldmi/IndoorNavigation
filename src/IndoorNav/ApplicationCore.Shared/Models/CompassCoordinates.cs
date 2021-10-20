using System;

namespace ApplicationCore.Shared.Models
{
    public class CompassCoordinates : IEquatable<CompassCoordinates>
    {
        public CompassCoordinates(double degree)
        {
            Degree = degree;
        }

        public double Degree { get; set; }

        
        public bool Equals(CompassCoordinates? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Degree.Equals(other.Degree);
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CompassCoordinates) obj);
        }
        public override int GetHashCode()
        {
            return Degree.GetHashCode();
        }
    }
}