using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Shared.Models;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.CheckPointModel.Trilateration.Spheres
{
    public class SphereCsvStatistic
    {
        private const string Separator = ";";

        //public static readonly string CsvHeader = $"{nameof(BeaconId)}{Separator}{nameof(LastSeen)}{Separator}{nameof(Center)}{Separator}{nameof(RangeList)}{Separator}{nameof(Radius)}{Separator}{nameof(ExpectedRadius)}";
        public static readonly string CsvHeader =
            $"{nameof(BeaconId)}{Separator}{nameof(LastSeen)}{Separator}Signals{Separator}{nameof(Radius)}{Separator}{nameof(ExpectedRadius)}";

        private SphereCsvStatistic(BeaconId beaconId, Point center, IReadOnlyList<RangeBle> rangeList, double radius,
            DateTimeOffset lastSeen, int expectedRadius)
        {
            BeaconId = beaconId;
            Center = center;
            RangeList = rangeList;
            Radius = radius;
            LastSeen = lastSeen;
            ExpectedRadius = expectedRadius;
        }

        public BeaconId BeaconId { get; }
        public Point Center { get; }
        public IReadOnlyList<RangeBle> RangeList { get; }
        public double Radius { get; }
        public DateTimeOffset LastSeen { get; }
        public int ExpectedRadius { get; }


        public static SphereCsvStatistic Create(Sphere sphere, int expectedRadius)
        {
            return new SphereCsvStatistic(sphere.BeaconId, sphere.Center, sphere.RangeList, sphere.Radius,
                sphere.LastSeen, expectedRadius);
        }

        public string Convert2CsvFormat()
        {
            var rangeStr = RangeList.Select(r => r.ToString()).Aggregate((s1, s2) => $"{s1}, {s2}");
            return
                $"{BeaconId.Major}/{BeaconId.Minor}{Separator}{LastSeen:hh:mm:ss}{Separator}{rangeStr}{Separator}{Radius:F1}{Separator}{ExpectedRadius:F1}";
        }
    }
}