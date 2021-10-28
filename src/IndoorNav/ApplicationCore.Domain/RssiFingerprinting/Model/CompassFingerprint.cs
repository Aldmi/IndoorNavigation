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
        public CompassFingerprint(CompassCoordinates compassCoordinate, IList<BeaconAverage> beaconAverages)
        {
            CompassCoordinate = compassCoordinate;
            BeaconAverages = beaconAverages;
        }

        public CompassCoordinates CompassCoordinate { get; }
        public IList<BeaconAverage> BeaconAverages { get; }
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherCf"></param>
        /// <returns></returns>
        public Result<SimilarCompassFingerprint> CreateSimilar (CompassFingerprint otherCf)
        {
            return CompassCoordinate.Equals(otherCf.CompassCoordinate) ?
                SimilarCompassFingerprint.Create(this, otherCf) :
                Result.Failure<SimilarCompassFingerprint>("Сторона света не совпадает при поиске похожести 2-ух CompassFingerprint");
        }
    }
}