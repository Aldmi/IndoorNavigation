using System;
using System.Collections.Generic;
using ApplicationCore.Domain.DistanceService;
using ApplicationCore.Domain.DistanceService.Model;
using ApplicationCore.Shared;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.CheckPointModel
{
    /// <summary>
    /// контрольная точка.
    /// Базовый класс.
    /// </summary>
    public abstract class CheckPointBase : IEquatable<CheckPointBase>
    {
        public CheckPointDescription Description { get; }
        public BeaconId BeaconId { get; }
        
        protected CheckPointBase(BeaconId beaconId, CheckPointDescription description)
        {
            Description = description;
            BeaconId = beaconId;
        }

        
        public abstract Zone GetZone(IEnumerable<BeaconDistance> distances);
        
        
        public bool Equals(CheckPointBase other)
        {
            return BeaconId.Equals(other.BeaconId);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Description, BeaconId);
        }
        public override string ToString() => $"{BeaconId.StrMajorMinor} {Description.Name}";
    }
}