using Libs.Beacons.Models;

namespace Libs.Beacons.Platforms
{
    public enum BeaconRegisterEventType
    {
        Add,
        Update,
        Remove,
        Clear
    }


    public class BeaconRegisterEvent
    {
        public BeaconRegisterEvent(BeaconRegisterEventType eventType, BeaconRegion? region = null)
        {
            Type = eventType;
            Region = region;
        }


        public BeaconRegisterEventType Type { get; }
        public BeaconRegion? Region { get; }
    }
}