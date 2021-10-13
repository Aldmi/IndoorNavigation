using System;
using System.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using ApplicationCore.Domain.DistanceService;
using Test.Beacons.Moq;
using Xunit;

namespace Test.Beacons.FlowsTest
{
    public class BeaconDistanceFlowTest
    {
        [Fact]
        public async Task HandlingBeaconGroupsTest()
        { 
            //arrange   
            var beaconsFlow= BeaconFlowData.CreateFlowImmediatly_ForOneBeacon();

            //act
            var resultFlow = beaconsFlow.Beacon2BeaconDistance(
                TimeSpan.FromSeconds(0.6),
                1.0,
                null);
            var beaconDistances= (await resultFlow.ToTask()).ToList();
          
            //assert
        }
    }
}