using Libs.Beacons.Models;

namespace ApplicationCore.Domain.DiscreteSteps
{
    /// <summary>
    /// Входные данные.
    /// </summary>
    public class InputData
    {
        public BeaconId BeaconId { get;}
        public double Range { get;}
        
        public InputData(BeaconId beaconId, double range)
        {
            BeaconId = beaconId;
            Range = range;
        }
    }
}