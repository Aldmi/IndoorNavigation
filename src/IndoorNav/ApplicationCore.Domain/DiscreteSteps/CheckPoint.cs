using System;
using ApplicationCore.Domain.Services;
using ApplicationCore.Shared;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.DiscreteSteps
{
    /// <summary>
    /// контрольная точка
    /// </summary>
    public class CheckPoint : IEquatable<CheckPoint>
    {
        public CheckPointDescription Description { get; }
        public BeaconId BeaconId { get; }
        public CoverageArea Area { get; }

        
        public CheckPoint(
            BeaconId beaconId,
            CheckPointDescription description,
            CoverageArea area)
        {
            BeaconId = beaconId;
            Description = description;
            Area = area;
        }


        /// <summary>
        /// Вернуть статус попадания в зону действия маяка для входных дангных.
        /// Если Id не совпадает, то зона Unknown
        /// Если Id совпал, то определяем, внутри зоны маяка находимся или вне зоны.
        /// </summary>
        public Zone GetZone(BeaconDistanceModel inputData)
        {
            return inputData.BeaconId == BeaconId ?
                Area.GetZone(inputData.Distance) :
                Zone.Unknown;
        }

        
        public bool Equals(CheckPoint other)
        {
            return BeaconId.Equals(other.BeaconId);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Description, BeaconId, Area);
        }
    }
}