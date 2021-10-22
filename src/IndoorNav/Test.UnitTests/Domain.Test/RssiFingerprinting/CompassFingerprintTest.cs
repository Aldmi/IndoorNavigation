using System;
using System.Collections.Generic;
using ApplicationCore.Domain.RssiFingerprinting.Model;
using ApplicationCore.Shared.Models;
using Libs.Beacons.Models;
using Xunit;

namespace Test.Beacons.Domain.Test.RssiFingerprinting
{
    public class CompassFingerprintTest
    {
        [Fact]
        public void GetSimilarTest()
        {
            //arrange
            var uuid = Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e");
            ushort major = 1;
            var txPower = -59;
            var cf1 = new CompassFingerprint(
                new CompassCoordinates(60.0),
                new List<BeaconAverage>
                {
                    new(new BeaconId(uuid, major, 1), -65.6, txPower),
                    new(new BeaconId(uuid, major, 2), -85.1, txPower),
                    new(new BeaconId(uuid, major, 3), -77.8, txPower)
                });
            
            var cf2 = new CompassFingerprint(
                new CompassCoordinates(60.0),
                new List<BeaconAverage>
                {
                    new(new BeaconId(uuid, major, 1), -60.6, txPower),
                    new(new BeaconId(uuid, major, 2), -66.1, txPower),
                    new(new BeaconId(uuid, major, 3), -59.8, txPower)
                });

            //act
           var similarRes= cf1.GetSimilar(cf2);


            //assert
        }
    }
}