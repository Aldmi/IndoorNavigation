using Libs.Beacons.Models;

namespace ApplicationCore.Domain.DiscreteSteps
{
    public class InputData
    {
        public InputData(BeaconId beaconId, double range)
        {
            BeaconId = beaconId;
            Range = range;
        }

        public BeaconId BeaconId { get;}
        public double Range { get;}
    }
}