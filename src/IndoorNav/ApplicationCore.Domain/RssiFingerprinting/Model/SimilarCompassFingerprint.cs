using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Shared.Helpers;
using ApplicationCore.Shared.Models;
using CSharpFunctionalExtensions;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.RssiFingerprinting.Model
{
    /// <summary>
    /// Похожесть отпечатков
    /// </summary>
    public class SimilarCompassFingerprint
    {
        /// <summary>
        /// Список отличий от референсных отпечатков.
        /// </summary>
        public IList<DifferenceBeaconAverage> DifferenceFingerprints { get; }
        
        /// <summary>
        /// Отсутствующие отпечатки.
        /// Список отпечатков которых нет в искомом Fp относительно Референсного
        /// </summary>
        public IList<BeaconAverage> MissingFingerprints { get; }
        
        /// <summary>
        /// Болльше отсутствующих отпечатков
        /// </summary>
        public bool MoreMissed(SimilarCompassFingerprint s) => MissingFingerprints.Count > s.MissingFingerprints.Count;
      
        
        
        private SimilarCompassFingerprint(
            IList<DifferenceBeaconAverage> differenceFingerprints,
            IList<BeaconAverage> missingFingerprints)
        {
            DifferenceFingerprints = differenceFingerprints;
            MissingFingerprints = missingFingerprints;
        }
        
        /// <summary>
        /// Создать объект Похожесть отпечатков.
        /// </summary>
        /// <param name="referenseFp">Базовый отпечаток</param>
        /// <param name="fp">Отпечаток для сравнения</param>
        public static Result<SimilarCompassFingerprint> Create(CompassFingerprint referenseFp, CompassFingerprint fp)
        {
            var differenceList = new List<DifferenceBeaconAverage>();
            var missingList = new List<BeaconAverage>();
            foreach (var refBa in referenseFp.BeaconAverages)
            {
                var ba= fp.BeaconAverages.FirstOrDefault(b => b.Id.Equals(refBa.Id));
                if (ba == null)
                {
                    missingList.Add(refBa); 
                }
                else
                {
                    var deltaRssi = Math.Abs(refBa.Rssi) - Math.Abs(ba.Rssi);
                    var refDistanceRes = RssiHelpers.CalculateDistance(refBa.TxPower, refBa.Rssi);
                    var distanceRes = RssiHelpers.CalculateDistance(ba.TxPower, ba.Rssi);
                    var (_, isFailure, error) = Result.Combine(refDistanceRes, distanceRes)
                        .Bind(() =>
                        {
                            var deltaDistance = refDistanceRes.Value - distanceRes.Value;
                            var dr = new DifferenceBeaconAverage(refBa.Id, deltaRssi, deltaDistance);
                            differenceList.Add(dr);
                            return Result.Success();
                        });
                    if (isFailure)
                        return Result.Failure<SimilarCompassFingerprint>(error);
                }
            }
            return new SimilarCompassFingerprint(differenceList, missingList);
        }
        
        
        
        
        /// <summary>
        /// Наиболее хорошая дельта, Rssi может сильно отличаться от референсного, он должен быть лутше (ближе) референсного
        /// </summary>
        public bool BetterDeltaRssi (SimilarCompassFingerprint other)=> CompareDifferenceFingerprints(other,
            (s1Diff, s2Diff) => s1Diff.DeltaRssi > s2Diff.DeltaRssi);
        
        
        /// <summary>
        /// Наименьшая дельта, сильнее похож на рефернесный.
        /// </summary>
        public bool SmallestDeltaRssi (SimilarCompassFingerprint other)=> CompareDifferenceFingerprints(other,
                (s1Diff, s2Diff) => Math.Abs(s1Diff.DeltaRssi) < Math.Abs(s2Diff.DeltaRssi));
        
        
       
        /// <summary>
        /// Сравнить 2 отпечтка.
        /// </summary>
        private bool CompareDifferenceFingerprints(SimilarCompassFingerprint other, Func<DifferenceBeaconAverage, DifferenceBeaconAverage, bool> condition)
        {
            ushort thisGrade = 0;
            ushort otherGrade = 0;
            foreach (var diff in DifferenceFingerprints)
            {
                var findDiff= other.DifferenceFingerprints.FirstOrDefault(d => d.BeaconId.Equals(diff.BeaconId));
                if (findDiff == null || condition(diff, findDiff))
                {
                    thisGrade++;
                }
                else
                {
                    otherGrade++;
                }
            }
            return thisGrade > otherGrade;
        }
        
        
        
        /// <summary>
        /// Различия между 2-мя BeaconAverage.
        /// считаем по модуюлю.
        /// если разность положительная, ближе относительно рефернсного.
        /// если разность отрицательная, дальше относительно рефернсного.
        /// </summary>
        public class DifferenceBeaconAverage
        {
            public DifferenceBeaconAverage(BeaconId beaconId, double deltaRssi, double deltaDistance)
            {
                BeaconId = beaconId;
                DeltaRssi = deltaRssi;
                DeltaDistance = deltaDistance;
                DistanceGrade = DeltaRssi > 0 ? DistanceGrade.Closer : DistanceGrade.Further;
            }

            public BeaconId BeaconId { get; }
            public double DeltaRssi { get; }
            public double DeltaDistance { get; }
            public DistanceGrade DistanceGrade { get; }
        }
    }
}