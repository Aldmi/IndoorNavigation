using System;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.DistanceService.Model
{
    /// <summary>
    /// Данные от Beacon, обработанные и преобразованнные к модели расстояния. 
    /// </summary>
    public class BeaconDistance : IEquatable<BeaconDistance>
    {
        public BeaconId BeaconId { get; }
        public double Distance { get; }
        public DateTimeOffset LastSeen { get; }

        public BeaconDistance(BeaconId beaconId, double distance)
        {
            BeaconId = beaconId;
            Distance = distance;
            LastSeen = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// Протух.
        /// LastSeen устарелло
        /// </summary>
        public bool IsRotten(TimeSpan validOffset)
        {
            var offset = DateTimeOffset.UtcNow - LastSeen;
            return offset > validOffset;
        }
        
        
        public override string ToString() => $"{Distance:F1}м";

        
        public bool Equals(BeaconDistance? other)
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
            return Equals((BeaconDistance) obj);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(BeaconId, Distance, LastSeen);
        }
    }
}