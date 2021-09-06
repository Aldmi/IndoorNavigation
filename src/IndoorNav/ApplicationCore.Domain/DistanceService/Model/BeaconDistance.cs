using System;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.DistanceService.Model
{
    /// <summary>
    /// Данные от Beacon, обработанные и пмреобразованнные к модели расстояния. 
    /// </summary>
    public class BeaconDistance
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
    }
}