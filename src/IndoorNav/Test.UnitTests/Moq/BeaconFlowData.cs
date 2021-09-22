using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Libs.Beacons.Models;

namespace Test.Beacons.Moq
{
    public static class BeaconFlowData
    {
        /// <summary>
        /// 
        /// </summary>
        public static IObservable<Beacon> CreateFlowImmediatly_ForOneBeacon()
        {
            var listBeacons= new List<Beacon>
            {
                CreateBeaconMagjor1(1, -56, -50),
                CreateBeaconMagjor1(1, -52, -50),
                CreateBeaconMagjor1(1, -54, -50),
                CreateBeaconMagjor1(1, -60, -50),
                CreateBeaconMagjor1(1, -52, -50),
            };
            var sourse = listBeacons.ToObservable();
            return sourse;
        }


        public static Beacon CreateBeaconMagjor1(ushort minor, int rssi, int txPower)
        {
            return new(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, minor), rssi, txPower);
        }
    }
}