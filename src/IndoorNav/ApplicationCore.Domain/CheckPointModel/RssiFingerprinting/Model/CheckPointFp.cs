using System.Collections.Generic;
using ApplicationCore.Domain.DistanceService.Model;
using ApplicationCore.Shared;
using ApplicationCore.Shared.Models;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.CheckPointModel.RssiFingerprinting.Model
{
    /// <summary>
    /// контрольная точка.
    /// для алгоритма RssiFingerprinting
    /// </summary>
    public class CheckPointFp : CheckPointBase
    {
        public CheckPointFp(CheckPointDescription description) : base(description)
        {
        }

        public override Zone GetZone(IEnumerable<BeaconDistance> distances)
        {
            throw new System.NotImplementedException();
        }
    }
}