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
        public IList<DifferenceRelativeReferenceBeaconAverage> DifferenceFingerprints { get; }
        
        /// <summary>
        /// Отсутствующие отпечатки.
        /// Список отпечатков которых нет в искомом Fp относительно Референсного
        /// </summary>
        public IList<BeaconAverage> MissingFingerprints { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public bool MoreMissed(SimilarCompassFingerprint s) => MissingFingerprints.Count > s.MissingFingerprints.Count;
      

        /// <summary>
        /// 
        /// </summary>
        public bool CompareDifferenceFingerprints(SimilarCompassFingerprint s, Func<DifferenceRelativeReferenceBeaconAverage,DifferenceRelativeReferenceBeaconAverage, bool> condition)
        {
            ushort thisGrade = 0;
            ushort otherGrade = 0;
            foreach (var diff in DifferenceFingerprints)
            {
                var findDiff= s.DifferenceFingerprints.FirstOrDefault(d => d.BeaconId.Equals(diff.BeaconId));
                if (condition(diff, findDiff))
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

        
        private SimilarCompassFingerprint(
            IList<DifferenceRelativeReferenceBeaconAverage> differenceFingerprints,
            IList<BeaconAverage> missingFingerprints)
        {
            DifferenceFingerprints = differenceFingerprints;
            MissingFingerprints = missingFingerprints;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="referenseFp"></param>
        /// <param name="fp"></param>
        /// <returns></returns>
        public static Result<SimilarCompassFingerprint> Create(CompassFingerprint referenseFp, CompassFingerprint fp)
        {
            var differenceList = new List<DifferenceRelativeReferenceBeaconAverage>();
            var missingList = new List<BeaconAverage>();
            for (var i = 0; i < referenseFp.Fingerprints.Count; i++)
            {
                var refBa = referenseFp.Fingerprints[i];
                var ba = fp.Fingerprints[i];
                //Если отпечаток найден в списке референсных, то вычислим разницу отпечатков 
                if (refBa.Equals(ba))
                {
                    var deltaRssi = refBa.Rssi - ba.Rssi;
                    var refDistanceRes = RssiHelpers.CalculateDistance(refBa.TxPower, refBa.Rssi);
                    var distanceRes = RssiHelpers.CalculateDistance(ba.TxPower, ba.Rssi);
                    var res = Result.Combine(refDistanceRes, distanceRes)
                        .Bind(() =>
                        {
                            var deltaDistance = refDistanceRes.Value - distanceRes.Value;
                            var dr = new DifferenceRelativeReferenceBeaconAverage(refBa.Id, deltaRssi, deltaDistance);
                            differenceList.Add(dr);
                            return Result.Success();
                        });
                    if (res.IsFailure)
                        return Result.Failure<SimilarCompassFingerprint>(res.Error);
                }
                //Если не найден, то поместим отпечаток в список отсутсвующих.
                else
                {
                    missingList.Add(ba);
                }
            }
            return new SimilarCompassFingerprint(differenceList, missingList);
        }
        
        
        
        public static bool operator > (SimilarCompassFingerprint s1, SimilarCompassFingerprint s2)
        {
            // //сравнение кол-во пропущенных отпечатков
            // if (s1.MoreMissed(s2))
            // {
            //     return false; //s1 хуже s2 
            // }
     
            //сравним по DeltaRssi.
            return s1.CompareDifferenceFingerprints(s2,
                (s1Diff, s2Diff) => s1Diff.DeltaRssi > s2Diff.DeltaRssi);
        }

        
        public static bool operator < (SimilarCompassFingerprint s1, SimilarCompassFingerprint s2)
        {
            return !(s1 > s2);
        }
        
        
        
        /// <summary>
        /// Различия между 2-мя BeaconAverage.
        /// считаем по модуюлю.
        /// если разность положительная, то референсное значение больше чем измеряемое (ближе)
        /// если разность отрицательная, то референсное значение меньше чем измеряемое (дальше) 
        /// </summary>
        public class DifferenceRelativeReferenceBeaconAverage
        {
            public DifferenceRelativeReferenceBeaconAverage(BeaconId beaconId, double deltaRssi, double deltaDistance)
            {
                BeaconId = beaconId;
                DeltaRssi = deltaRssi;
                DeltaDistance = deltaDistance;
                DistanceGrade = DeltaDistance > 0 ? DistanceGrade.Closer : DistanceGrade.Further;
            }

            public BeaconId BeaconId { get; }
            public double DeltaRssi { get; }
            public double DeltaDistance { get; }
            public DistanceGrade DistanceGrade { get; }
        }
    }
}