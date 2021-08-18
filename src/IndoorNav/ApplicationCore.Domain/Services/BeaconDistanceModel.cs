using System;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.Services
{
    /// <summary>
    /// Данные от Beacon, обработанные и пмреобразованнные к модели расстояния. 
    /// </summary>
    public class BeaconDistanceModel
    {
        public BeaconId BeaconId { get; }
        public double Distance { get; }
        public DateTimeOffset LastSeen { get; }
        
        public BeaconDistanceModel(BeaconId beaconId, double distance)
        {
            BeaconId = beaconId;
            Distance = distance;
            LastSeen = DateTimeOffset.UtcNow;
        }
    }
}