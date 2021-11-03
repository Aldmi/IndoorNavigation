using System.Collections.Generic;

using ApplicationCore.Domain.RssiFingerprinting.Model;
using ApplicationCore.Domain.RssiFingerprinting.Services;
using ApplicationCore.Shared.Models;
using FluentAssertions;
using Libs.Beacons.Models;
using Test.Beacons.Domain.Test.RssiFingerprinting.Data;
using Xunit;

namespace Test.Beacons.Domain.Test.RssiFingerprinting
{
    public class FindSimilarTotalFingerprintServiceTest
    {
        
        [Fact]
        public void FindTotalFingerprintTest()
        {
            //arrange
            var totalList = TotalFingerprintDatas.GetTotalFingerprint_3Fp_Linear_on_Y_moving();
            var cf = new CompassFingerprint(CompassCoordinates.North, new List<BeaconAverage>
            {
                TotalFingerprintDatas.CreateBeaconAverage(1, 71.6),
                TotalFingerprintDatas.CreateBeaconAverage(2, -87.1),
                TotalFingerprintDatas.CreateBeaconAverage(3, -77.8),
                TotalFingerprintDatas.CreateBeaconAverage(4, -61.1)
            });
            var expectedRoomCoordinate = new Point(1, 3);

            //act
            var tfRes= FindSimilarTotalFingerprintService.FindTotalFingerprint(totalList, cf);

            //assert
            tfRes.IsSuccess.Should().BeTrue();
            tfRes.Value.tf.RoomCoordinate.Should().Be(expectedRoomCoordinate);
        }
    }
}