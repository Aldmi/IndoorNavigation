using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Domain.CheckPointModel.Trilateration.Spheres;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.DistanceService.Model
{
    public class BeaconDistanceStatisticCsv
    {
        private const string Separator = ";";
        public static readonly string CsvHeader =
            $"{nameof(BeaconId)}{Separator}{nameof(LastSeen)}{Separator}{nameof(DistanceList)}{Separator}{nameof(DistanceResult)}{Separator}{nameof(DistanceResultExpected)}";

        
        public BeaconDistanceStatisticCsv(BeaconId beaconId, IReadOnlyList<double> distanceList, double distanceResult, DateTimeOffset lastSeen, double distanceResultExpected)
        {
            BeaconId = beaconId;
            DistanceList = distanceList;
            DistanceResult = distanceResult;
            LastSeen = lastSeen;
            DistanceResultExpected = distanceResultExpected;
        }


        public BeaconId BeaconId { get; }
        public IReadOnlyList<double> DistanceList { get; }
        public double DistanceResult { get; }
        public double DistanceResultExpected { get; }
        public DateTimeOffset LastSeen { get; }
      
        
        
        
        public static BeaconDistanceStatisticCsv Create(BeaconDistanceStatistic distanceStatistic, double distanceResultExpected)
        {
            return new BeaconDistanceStatisticCsv(
                distanceStatistic.BeaconId,
                distanceStatistic.DistanceList,
                distanceStatistic.DistanceResult,
                distanceStatistic.LastSeen,
                distanceResultExpected);
        }
        
        
        public string Convert2CsvFormat()
        {
            var distanceList = DistanceList.Select(r => r.ToString("F1")).Aggregate((s1, s2) => $"{s1} / {s2}");
            return
                $"{BeaconId.Major}/{BeaconId.Minor}{Separator}{LastSeen:hh:mm:ss:ffff}{Separator}{distanceList}{Separator}{DistanceResult:F1}{Separator}{DistanceResultExpected:F1}";
        }
    }
}