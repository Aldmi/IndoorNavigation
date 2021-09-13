using System.Collections.Generic;
using ApplicationCore.Domain.DistanceService.Model;
using ApplicationCore.Shared;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.CheckPointModel.RssiFingerprinting.Model
{
    /// <summary>
    /// контрольная точка.
    /// для алгоритма RssiFingerprinting
    /// </summary>
    public class CheckPointFp : CheckPointBase
    {
        public CheckPointFp(BeaconId beaconId, CheckPointDescription description) : base(beaconId, description)
        {
        }

        public override Zone GetZone(IEnumerable<BeaconDistance> distances)
        {
            throw new System.NotImplementedException();
        }
    }
}