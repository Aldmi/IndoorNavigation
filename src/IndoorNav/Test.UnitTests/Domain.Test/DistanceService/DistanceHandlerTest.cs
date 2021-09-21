using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Domain.DistanceService.Services;
using Libs.Beacons.Models;
using Xunit;

namespace Test.Beacons.Domain.Test.DistanceService
{
    public class DistanceHandlerTest
    {
        
        private readonly List<DistanceListByBeacon> _signals = new()
        {
             new()
             {
                 Id = new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1),
                 Distances = new List<double> {2.1, 2.2, 3.0}
             },
             new()
             {
                 Id = new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1),
                 Distances = new List<double> {3.1, 3.2, 4.0}
             }
        };


        [Fact]
        public async Task AggregateDistanceTest()
        {
            //arrange
            var distanceHandler= DistanceHandlerFactory.CreateRemoveDifferentAndAverageAggergate();

            //act
            foreach (var byBeaconSignals in _signals)
            {
                var aggregateDist = distanceHandler.Aggregate(byBeaconSignals.Id, byBeaconSignals.Distances);
                await Task.Delay(1000);
            }


            //assert
        }
    }
}