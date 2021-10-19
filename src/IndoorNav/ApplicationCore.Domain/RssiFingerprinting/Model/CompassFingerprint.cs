using System.Collections.Generic;
using ApplicationCore.Shared.Models;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.RssiFingerprinting.Model
{
    /// <summary>
    /// Отпечаток в точке от ВСЕХ ближайщих Beacon.
    /// С координтами по компасу.
    /// </summary>
    public class CompassFingerprint
    {
        public CompassFingerprint(CompassCoordinates compassCoordinates, IList<BeaconAverage> fingerprint)
        {
            CompassCoordinates = compassCoordinates;
            Fingerprint = fingerprint;
        }

        public CompassCoordinates CompassCoordinates { get; }
        public IList<BeaconAverage> Fingerprint { get;  }
        
        
        // /// <summary>
        // /// Маска всех сигналов в этой точке, по сторонам света.
        // /// спсисок 
        // /// </summary>
        // private Dictionary<int, IList<BeaconAverage>> Mask { get; }
    }
}