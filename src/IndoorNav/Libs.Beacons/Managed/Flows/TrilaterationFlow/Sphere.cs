using System;
using System.Collections.Generic;
using Libs.Beacons.Managed.Domain;
using Libs.Beacons.Models;

namespace Libs.Beacons.Managed.Flows.TrilaterationFlow
{
    public class Sphere : IEquatable<Sphere>
    {
        public Sphere(BeaconId beaconId, Point center, IReadOnlyList<RangeBle> rangeList)
        {
            BeaconId = beaconId;
            Center = center;
            RangeList = rangeList;
        }
        
        
        public BeaconId BeaconId { get; }
        public Point Center { get; }
        public IReadOnlyList<RangeBle> RangeList { get; }
        public double Radius => RangeBle.CalcAverageValue(RangeList);
        public bool CenterIsEmpty => Center == Point.EmptyPoint;


        public bool Equals(Sphere? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return BeaconId.Equals(other.BeaconId) && Center.Equals(other.Center) && Radius.Equals(other.Radius);
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
            return HashCode.Combine(BeaconId, Center, Radius);
        }
    }
}