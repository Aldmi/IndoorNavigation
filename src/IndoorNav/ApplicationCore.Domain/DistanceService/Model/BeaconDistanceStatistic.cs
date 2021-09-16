using System;
using System.Collections.Generic;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.DistanceService.Model
{
    /// <summary>
    /// Статистика, обработки списка distanceList от маяка и преобразования его к одному сигналу distanceResult.
    /// </summary>
    public class BeaconDistanceStatistic
    {
        public BeaconDistanceStatistic(BeaconId beaconId, IReadOnlyList<double> distanceList, double distanceResult)
        {
            BeaconId = beaconId;
            DistanceList = distanceList;
            DistanceResult = distanceResult;
            LastSeen = DateTimeOffset.UtcNow;
        }

        public BeaconId BeaconId { get; }
        public IReadOnlyList<double> DistanceList { get; }
        public double DistanceResult { get; }
  
        public DateTimeOffset LastSeen { get; }
    }
}