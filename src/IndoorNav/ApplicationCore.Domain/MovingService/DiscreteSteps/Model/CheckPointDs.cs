using System;
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
        public override Zone GetZone(BeaconDistance inputData)
        {
            return inputData.BeaconId == BeaconId ?
                Area.GetZone(inputData.Distance) :
                Zone.Unknown;
        }
        
        
        public override string ToString() => $"{BeaconId.StrMajorMinor} {Description.Name} {Area}";
    }
}