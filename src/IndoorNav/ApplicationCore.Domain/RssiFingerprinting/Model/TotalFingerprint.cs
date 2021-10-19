using System.Collections.Generic;
using ApplicationCore.Shared.Models;

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
            IList<CompassFingerprint> fingerprints)
        {
            RoomCoordinate = roomCoordinate;
            Fingerprints = fingerprints;
        }

        /// <summary>
        /// Координата отпечатка в помещении.
        /// </summary>
        public Point RoomCoordinate { get; }

        /// <summary>
        /// Отпечатки по сторонам света
        /// </summary>
        public IList<CompassFingerprint> Fingerprints { get; }
    }
}