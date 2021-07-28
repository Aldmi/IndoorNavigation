using System;
using Libs.Beacons.Models;

namespace Libs.Beacons.Managed.TrilaterationFlow
{
    public class Sphere : IEquatable<Sphere>
    {
        public Sphere(Beacon beacon) => Beacon = beacon;
        public Beacon Beacon { get; }
        
        
        public Point Center { get; private set; }
        public double Radius { get; private set;}


        internal void SetCenter(Point center)
        {
            Center = center;
        }
        
        
        internal void SetRadius(double radius)
        {
            Radius = radius;
        }

        
        public bool Equals(Sphere? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Beacon.Equals(other.Beacon) && Center.Equals(other.Center) && Radius.Equals(other.Radius);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Sphere) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Beacon, Center, Radius);
        }
    }
}