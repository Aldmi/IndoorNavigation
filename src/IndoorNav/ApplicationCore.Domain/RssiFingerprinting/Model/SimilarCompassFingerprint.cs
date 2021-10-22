using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Shared.Models;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.RssiFingerprinting.Model
{
    /// <summary>
    /// Похожесть отпечатков
    /// </summary>
    public class SimilarCompassFingerprint
    {
        /// <summary>
        /// Референсный отпечаток (относительно которого производим сравнение)
        /// </summary>
        public CompassFingerprint ReferenceFp { get; }
        
        /// <summary>
        /// Отпечаток для сравнения
        /// </summary>
        public CompassFingerprint Fp { get; }
        
        
        /// <summary>
        /// Отсутствующие отпечатки.
        /// Список отпечатков которых нет в искомом Fp относительно Референсного
        /// </summary>
        public IList<BeaconAverage> MissingFingerprints { get; }
        
        
        
        public static bool operator > (SimilarCompassFingerprint s1, SimilarCompassFingerprint s2)
        {
            return true;
        }

        public static bool operator <(SimilarCompassFingerprint s1, SimilarCompassFingerprint s2)
        {
            return false;
        }
        
        
        public static SimilarCompassFingerprint Create(CompassFingerprint referenseFp, CompassFingerprint fp)
        {
            var distinctList = new List<BeaconAverage>();
            for (var i = 0; i < referenseFp.Fingerprints.Count; i++)
            {
                var refBa = referenseFp.Fingerprints[i];
                var ba = fp.Fingerprints[i];
                if (!refBa.Equals(ba))
                {
                    distinctList.Add(ba);
                }
            }
            
            return new SimilarCompassFingerprint();
        }
        
        
        /// <summary>
        /// Различия между 2-мя BeaconAverage.
        /// считаем по модуюлю.
        /// если разность положительная, то референсное значение больше чем измеряемое (ближе)
        /// если разность отрицательная, то референсное значение меньше чем измеряемое (дальше) 
        /// </summary>
        private class DifferenceRelativeReferenceBeaconAverage
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