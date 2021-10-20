using System.Collections.Generic;
using ApplicationCore.Shared.Models;
using CSharpFunctionalExtensions;

namespace ApplicationCore.Domain.RssiFingerprinting.Model
{
    /// <summary>
    /// Конечный отпечаток, снятый по разным сторонам света (разныее координаты компаса)
    /// Привязанный к координатам помещения 
    /// </summary>
    public class TotalFingerprint
    {
        public TotalFingerprint(
            Point roomCoordinate,
            Dictionary<CompassCoordinates, CompassFingerprint> mask)
        {
            RoomCoordinate = roomCoordinate;
            Mask = mask;
        }

        /// <summary>
        /// Координата отпечатка в помещении.
        /// </summary>
        public Point RoomCoordinate { get; }

        /// <summary>
        /// Отпечатки по сторонам света
        /// </summary>
        public Dictionary<CompassCoordinates, CompassFingerprint> Mask { get; }


        
        public Result<SimilarCompassFingerprint> CalcSimilarCompassFingerprint(CompassFingerprint cf)
        {
           return GetCompassFingerprint(cf).Bind(totalCf => totalCf.GetSimilar(cf));
        }


        /// <summary>
        /// Вернуть значение из словаря по ключу (стороне света)
        /// </summary>
        private Result<CompassFingerprint> GetCompassFingerprint(CompassFingerprint cf)
        {
            return Mask.TryGetValue(cf.CompassCoordinate, out var resCf) ?
                resCf :
                Result.Failure<CompassFingerprint>($"CompassFingerprint не найденн в словаре Mask по ключу {cf}");
        }
    }
}