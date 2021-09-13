using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Domain.DistanceService.Model;
using ApplicationCore.Shared;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.CheckPointModel.DiscreteSteps
{
    /// <summary>
    /// Модель маяка
    /// </summary>
    public class BeaconCheckPointItem
    {
        public BeaconId BeaconId { get; }
        public CoverageArea Area { get; }

        
        public BeaconCheckPointItem(BeaconId beaconId, CoverageArea area)
        {
            Area = area;
        }
        
        

        /// <summary>
        /// Вернуть статус попадания в зону действия маяка для входных дангных.
        /// Если Id не совпадает, то зона Unknown
        /// Если Id совпал, то определяем, внутри зоны маяка находимся или вне зоны.
        /// </summary>
        public Zone GetZone(IEnumerable<BeaconDistance> distances)
        {
            var distance= distances.FirstOrDefault(d => d.BeaconId == BeaconId);
            return distance != null ? Area.GetZone(distance.Distance) : Zone.Unknown;
        }


        public override string ToString() => $"{BeaconId.StrMajorMinor} {Area}";
    }
}