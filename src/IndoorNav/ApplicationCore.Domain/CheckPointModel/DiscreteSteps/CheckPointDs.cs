using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Domain.DistanceService.Model;
using ApplicationCore.Shared;
using ApplicationCore.Shared.Models;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.CheckPointModel.DiscreteSteps
{
    /// <summary>
    /// контрольная точка.
    /// для алгоритма DiscreteSteps
    /// </summary>
    public class CheckPointDs : CheckPointBase
    {
        private readonly BeaconCheckPointItem[] _checkPointItems;
        
        public CheckPointDs(CheckPointDescription description, BeaconCheckPointItem checkPointItem)
            : base(description)
        {
            _checkPointItems = new[] {checkPointItem};
        }
        public CheckPointDs(CheckPointDescription description, BeaconCheckPointItem[] checkPointItems)
            : base(description)
        {
            _checkPointItems = checkPointItems;
        }
        
        

        /// <summary>
        /// Вернуть статус попадания в зону действия маяка для входных данных.
        /// Если Id не совпадает, то зона Unknown
        /// Если Id совпал, то определяем, внутри зоны маяка находимся или вне зоны.
        /// </summary>
        public override Zone GetZone(IEnumerable<BeaconDistance> distances)
        {
            var zones = _checkPointItems
                .Select(ch => ch.GetZone(distances)).ToArray();

            //Хотя бы 1 в зоне - значит В зоне. (приоритетно попадание в зону)
            if (zones.Any(z => z == Zone.In))
                return Zone.In;
            
            //Хотя бы 1 вышел из зоны, значит ВНЕ зоны. (если Zone.In нету, и есть хотя бы 1 Zone.Out)
            if (zones.Any(z => z == Zone.Out))
                return Zone.Out;

            return Zone.Unknown;
        }
        
        
        //
        // /// <summary>
        // /// Вернуть статус попадания в зону действия маяка для входных данных.
        // /// Если Id не совпадает, то зона Unknown
        // /// Если Id совпал, то определяем, внутри зоны маяка находимся или вне зоны.
        // /// </summary>
        // public override Zone GetZone(IEnumerable<BeaconDistance> distances)
        // {
        //     foreach (var ch in _checkPointItems)
        //     {
        //         var distance= distances.FirstOrDefault(d => d.BeaconId == ch.BeaconId);
        //         var zone= distance != null ?  ch.Area.GetZone(distance.Distance) : Zone.Unknown;
        //         if (zone == Zone.In)
        //             return zone;
        //     }
        //     return Zone.Unknown;
        // }


        public override string ToString() => $"{base.ToString()} {Description.Name}";
    }
}