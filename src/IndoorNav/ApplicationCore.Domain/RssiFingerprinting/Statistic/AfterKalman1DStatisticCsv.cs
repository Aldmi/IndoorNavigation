using System;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.RssiFingerprinting.Statistic
{
    public class AfterKalman1DStatisticCsv
    {
        public BeaconId BeaconId { get; }
        public double RssiBefore { get; }
        public double RssiAfter { get; }
        public double DistanceBefore { get; }
        public double DistanceAfter { get; }
        public DateTimeOffset LastSeen { get; }
        public double DistanceResultExpected { get; }

        
        public AfterKalman1DStatisticCsv(BeaconId beaconId, double rssiBefore, double rssiAfter, double distanceBefore, double distanceAfter, DateTimeOffset lastSeen, double distanceResultExpected)
        {
            BeaconId = beaconId;
            RssiBefore = rssiBefore;
            RssiAfter = rssiAfter;
            DistanceBefore = distanceBefore;
            DistanceAfter = distanceAfter;
            LastSeen = lastSeen;
            DistanceResultExpected = distanceResultExpected;
        }
        public static AfterKalman1DStatisticCsv Create(AfterKalman1DStatistic statistic, double distanceResultExpected)
        {
            return new(
                statistic.BeaconId,
                statistic.RssiBefore,
                statistic.RssiAfter,
                statistic.DistanceBefore,
                statistic.DistanceAfter,
                statistic.LastSeen,
                distanceResultExpected);
        }
        
        private const string Separator = ";";
        public static readonly string CsvHeader =
            $"{nameof(BeaconId)}{Separator}{nameof(LastSeen)}{Separator}{nameof(RssiBefore)}{Separator}{nameof(RssiAfter)}{Separator}{nameof(DistanceBefore)}{Separator}{nameof(DistanceAfter)}{Separator}{nameof(DistanceResultExpected)}";
        public string Convert2CsvFormat()
        {
            return
                $"{nameof(BeaconId)}{Separator}{LastSeen:hh:mm:ss:ffff}{Separator}{RssiBefore:F1}{Separator}{RssiAfter:F1}{Separator}{DistanceBefore:F1}{Separator}{DistanceAfter:F1}{Separator}{DistanceResultExpected:F1}";
        }
    }
}