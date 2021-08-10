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


        public Zone GetZone(InputData inputData)
        {
            //TODO:Проверка BeaconId
            return Area.GetZone(inputData.Range);
        }
    }
}