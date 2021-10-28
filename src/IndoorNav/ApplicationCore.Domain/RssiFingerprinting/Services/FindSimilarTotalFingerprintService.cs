using System.Collections.Generic;
using ApplicationCore.Domain.RssiFingerprinting.Model;
using CSharpFunctionalExtensions;

namespace ApplicationCore.Domain.RssiFingerprinting.Services
{
    public static class FindSimilarTotalFingerprintService
    {
        /// <summary>
        /// Найти максимально похожий отпечаток к cf из списка всех отпечатков totalList. 
        /// </summary>
        /// <param name="totalList">Карта всех отпечатков</param>
        /// <param name="cf">отпечаток для сравнения</param>
        /// <returns></returns>
        public static Result<(TotalFingerprint tf, SimilarCompassFingerprint similar)> FindSimilar(IEnumerable<TotalFingerprint> totalList, CompassFingerprint cf)
        {
            (TotalFingerprint tf, SimilarCompassFingerprint similar) max = default;
            foreach (var tf in totalList)
            {
               var (isSuccess, _, similar, error) = tf.CalcSimilarCompassFingerprint(cf);
               if (isSuccess)
               {
                   if (max == default)
                   {
                       max = (tf, similar);
                   }
                   else
                   if (max.similar!.SmallestDeltaRssi(similar))
                   {
                       max = (tf, similar);
                   }
               }
            }

            return max == default
                ? Result.Failure<(TotalFingerprint tf, SimilarCompassFingerprint similar)>("не найден ни один похожий отпечаток")
                : Result.Success(max);
            
        }
    }
}