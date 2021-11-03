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
        /// <returns>найденный, наиболее похожий отпечаток</returns>
        public static Result<(TotalFingerprint tf, SimilarCompassFingerprint similar)> FindTotalFingerprint(IEnumerable<TotalFingerprint> totalList, CompassFingerprint cf)
        {
            (TotalFingerprint tf, SimilarCompassFingerprint similar) maxSimilar = default;
            foreach (var tf in totalList)
            {
               var (isSuccess, _, similar, error) = tf.CalcSimilarCompassFingerprint(cf);
               if (isSuccess)
               {
                   if (maxSimilar == default)
                   {
                       maxSimilar = (tf, similar);
                   }
                   else
                   if (similar.SmallestDeltaRssi(maxSimilar.similar!)) //найденный similar сильнее похож на референсный отпечаток из спсика totalList
                   {
                       maxSimilar = (tf, similar);
                   }
               }
               else
               {
                   //логировать error "ошибка при вычислении похожести отпечатка tf от similar"
               }
            }
            return maxSimilar == default
                ? Result.Failure<(TotalFingerprint tf, SimilarCompassFingerprint similar)>("не найден ни один похожий отпечаток")
                : Result.Success(maxSimilar);
        }
    }
}