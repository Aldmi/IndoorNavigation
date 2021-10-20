using System;
using System.Collections.Generic;
using ApplicationCore.Shared.Models;
using CSharpFunctionalExtensions;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.RssiFingerprinting.Model
{
    /// <summary>
    /// Отпечаток в точке,по компасу, от ВСЕХ ближайщих Beacon.
    /// </summary>
    public class CompassFingerprint
    {
        public CompassFingerprint(CompassCoordinates compassCoordinate, IList<BeaconAverage> fingerprints)
        {
            CompassCoordinate = compassCoordinate;
            Fingerprints = fingerprints;
        }

        public CompassCoordinates CompassCoordinate { get; }
        public IList<BeaconAverage> Fingerprints { get; }
        
        
        public Result<SimilarCompassFingerprint> GetSimilar (CompassFingerprint c2)
        {
            return CompassCoordinate.Equals(c2.CompassCoordinate) ?
                SimilarCompassFingerprint.Create(this, c2) :
                Result.Failure<SimilarCompassFingerprint>("Сторона света не совпадает при разности 2-ух CompassFingerprint");
        }
    }
}