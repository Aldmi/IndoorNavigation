using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Libs.Beacons.Managed.Domain;
using Libs.Beacons.Models;

namespace UseCase.Trilateration.Model
{
    public class SphereStatistic
    {
        private const string Separator = ";";
        public static readonly string CsvHeader = $"{nameof(BeaconId)}{Separator}{nameof(Center)}{Separator}{nameof(Radius)}";

        public SphereStatistic(BeaconId beaconId, Point center, IReadOnlyList<RangeBle> rangeList, double radius)
        {
            BeaconId = beaconId;
            Center = center;
            RangeList = rangeList;
            Radius = radius;
        }

        public BeaconId BeaconId { get; }
        public Point Center { get; }
        public IReadOnlyList<RangeBle> RangeList { get; }
        public double Radius { get; }


        public static SphereStatistic Create(Sphere sphere)
        {
            return new SphereStatistic(sphere.BeaconId, sphere.Center, sphere.RangeList, sphere.Radius);
        }

        public string Convert2CsvFormat()
        {
            var rangeStr = RangeList.Select(r => r.ToString()).Aggregate((s1, s2) => $"'{s1}' '{s2}'");
            //return $"{BeaconId}{Separator}{Center}{Separator}{rangeStr}";
            return $"{BeaconId.Major}/{BeaconId.Minor}{Separator}{Center}{Separator}{Radius:F2}";
        }
    }
}