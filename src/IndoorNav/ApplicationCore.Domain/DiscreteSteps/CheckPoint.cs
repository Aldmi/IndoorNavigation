using ApplicationCore.Shared;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.DiscreteSteps
{
    /// <summary>
    /// Узел графа.
    /// </summary>
    public class CheckPoint
    {
        public CheckPointDescription Description { get; }
        public BeaconId BeaconId { get; }
        public CoverageArea Area { get; }

        
        public CheckPoint(
            BeaconId beaconId,
            CheckPointDescription description,
            CoverageArea area)
        {
            BeaconId = beaconId;
            Description = description;
            Area = area;
        }


        /// <summary>
        /// Вернуть статус попадания в зону действия маяка для входных дангных.
        /// Если Id не совпадает, то зона Unknown
        /// Если Id совпал, то определяем, внутри зоны маяка находимся или вне зоны.
        /// </summary>
        public Zone GetZone(InputData inputData)
        {
            return inputData.BeaconId == BeaconId ?
                Area.GetZone(inputData.Range) :
                Zone.Unknown;
        }
    }
}