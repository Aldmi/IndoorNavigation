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
        public Guid Id { get; }
        public CheckPointDescription Description { get; }

        
        protected CheckPointBase(CheckPointDescription description)
        {
            Id = Guid.NewGuid();
            Description = description;
        }

        
        public abstract Zone GetZone(IEnumerable<BeaconDistance> distances);
        
        
        public bool Equals(CheckPointBase other)
        {
            return Id.Equals(other.Id);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Description, Id);
        }
        public override string ToString() => $"{Id} {Description.Name}";
    }
}