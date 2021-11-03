using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using ApplicationCore.Shared.Models;
using FluentAssertions;
using Test.Beacons.Domain.Test.RssiFingerprinting.Data;
using Test.Beacons.Moq;
using UseCase.RssiFingerprinting.Flows;
using Xunit;

namespace Test.Beacons.FlowsTest
{
    public class TotalFingerprintFlowTest
    {
        [Fact]
        public async Task Beacon2TotalFingerprintTest()
        {
            //arrange   
            var beaconsFlow= BeaconFlowData.CreateFlow_Fingerprints_7Steps_2metersByOne();
            var totalFp = TotalFingerprintDatas.GetTotalFingerprint_3TFp_Linear_on_Y_moving_By_North();
            
            //act
            var resultFlow = beaconsFlow.Beacon2TotalFingerprint(
                TimeSpan.FromSeconds(0.1),
                totalFp,
                20);
            var tfListResult = await resultFlow.ToList().ToTask();


            for (var i = 0; i < tfListResult.Count; i++)
            {
                var tfRes = tfListResult[i];
                switch (i)
                {
                    case 0:
                    case 1:
                        tfRes.Value.RoomCoordinate.Should().Be(new Point(0, 0));
                        break;
                    
                    case 3:
                        tfRes.Value.RoomCoordinate.Should().Be(new Point(0, 0));
                        break;
                    
                    
                }
                
                tfRes.IsSuccess.Should().BeTrue();
                //tfRes.Value.RoomCoordinate.Should().Be();
            }

            //assert
           //tfResult.IsSuccess.Should().BeTrue();
            //tfResult.Value.RoomCoordinate.
        }
    }
}