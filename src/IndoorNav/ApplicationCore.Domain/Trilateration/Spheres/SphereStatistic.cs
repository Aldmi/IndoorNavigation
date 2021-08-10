using System;
using System.Collections.Generic;
using ApplicationCore.Shared;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.Trilateration.Spheres
{
    public class SphereStatistic
    {
        private const string Separator = ";";
        //public static readonly string CsvHeader = $"{nameof(BeaconId)}{Separator}{nameof(LastSeen)}{Separator}{nameof(Center)}{Separator}{nameof(RangeList)}{Separator}{nameof(Radius)}{Separator}{nameof(ExpectedRadius)}";
        public static readonly string CsvHeader = $"{nameof(BeaconId)}{Separator}{nameof(LastSeen)}{Separator}{nameof(Center)}{Separator}CountSignal{Separator}{nameof(Radius)}{Separator}{nameof(ExpectedRadius)}";

        private SphereStatistic(BeaconId beaconId, Point center, IReadOnlyList<RangeBle> rangeList, double radius, DateTimeOffset lastSeen, int expectedRadius)
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
        public int ExpectedRadius{ get; }


        public static SphereStatistic Create(Sphere sphere, int expectedRadius)
        {
            return new SphereStatistic(sphere.BeaconId, sphere.Center, sphere.RangeList, sphere.Radius, sphere.LastSeen, expectedRadius);
        }

        // public string Convert2CsvFormat()
        // {
        //     //var rangeStr = RangeList.Select(r => r.ToString()).Aggregate((s1, s2) => $"'{s1}' '{s2}'");
        //     var rangeStr = RangeList.Select(r => r.Rssi.ToString()).Aggregate((s1, s2) => $"'{s1}' '{s2}'");
        //     //return $"{BeaconId}{Separator}{Center}{Separator}{rangeStr}";
        //     return $"{BeaconId.Major}/{BeaconId.Minor}{Separator}{LastSeen:hh:mm:ss}{Separator}{Center}{Separator}{rangeStr}{Separator}{Radius:F2}{Separator}{ExpectedRadius:D}";
        // }
        
        public string Convert2CsvFormat()
        {
            //var rangeStr = RangeList.Select(r => r.ToString()).Aggregate((s1, s2) => $"'{s1}' '{s2}'");
            var count = RangeList.Count;
            //return $"{BeaconId}{Separator}{Center}{Separator}{rangeStr}";
            return $"{BeaconId.Major}/{BeaconId.Minor}{Separator}{LastSeen:hh:mm:ss}{Separator}{Center}{Separator}{count}{Separator}{Radius:F2}{Separator}{ExpectedRadius:D}";
        }
    }
}