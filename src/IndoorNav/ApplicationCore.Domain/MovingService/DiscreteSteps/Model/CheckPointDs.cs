using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Domain.CheckPointModel;
using ApplicationCore.Domain.DistanceService;
using ApplicationCore.Domain.DistanceService.Model;
using ApplicationCore.Shared;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.MovingService.DiscreteSteps.Model
{
    /// <summary>
    /// контрольная точка.
    /// для алгоритма DiscreteSteps
    /// </summary>
    public class CheckPointDs : CheckPointBase
    {
        public CoverageArea Area { get; }

        
        public CheckPointDs(BeaconId beaconId, CheckPointDescription description, CoverageArea area) 
            : base(beaconId, description)
        {
            Area = area;
        }
        
        

        /// <summary>
        /// Вернуть статус попадания в зону действия маяка для входных дангных.
        /// Если Id не совпадает, то зона Unknown
        /// Если Id совпал, то определяем, внутри зоны маяка находимся или вне зоны.
        /// </summary>
        public override Zone GetZone(IEnumerable<BeaconDistance> distances)
        {
           var distance= distances.FirstOrDefault(d => d.BeaconId == BeaconId);
           return distance != null ? Area.GetZone(distance.Distance) : Zone.Unknown;
        }


        public override string ToString() => $"{BeaconId.StrMajorMinor} {Description.Name} {Area}";
    }
}