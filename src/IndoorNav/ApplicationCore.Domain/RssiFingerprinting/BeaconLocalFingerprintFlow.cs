using System.Collections.Generic;
using ApplicationCore.Domain.RssiFingerprinting.Model;
using ApplicationCore.Shared.Models;

namespace ApplicationCore.Domain.RssiFingerprinting
{
    public class BeaconLocalFingerprintFlow
    {
        
        public BeaconLocalFingerprintFlow(
            Point coordinate,
            IReadOnlyList<RssiFingerprint> fingerprints)
        {
            Coordinate = coordinate;
            Fingerprints = fingerprints;
        }

        /// <summary>
        /// Координаты отпечатка
        /// </summary>
        public Point Coordinate { get; }

        /// <summary>
        /// Список отпечатков Rssi
        /// </summary>
        public IReadOnlyList<RssiFingerprint> Fingerprints { get; }
    }
}