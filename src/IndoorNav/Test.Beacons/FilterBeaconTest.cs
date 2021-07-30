using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using FluentAssertions;
using Libs.Beacons;
using Libs.Beacons.Managed.Flows.FilterFlow;
using Libs.Beacons.Models;
using Xunit;

namespace Test.Beacons
{
    public class FilterBeaconTest
    {
        [Fact]
        public async Task AverageFilterTest()
        {
            //Arrange
            var listBeacons= new List<Beacon>
            {
                new(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1, Proximity.Near, -86, 0.5, -77),
                new(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1, Proximity.Near, -88, 0.5, -77),
                new(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1, Proximity.Near, -89, 0.5, -77),
                
                new(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 2, Proximity.Near, -91, 0.5,-77),
                new(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 2, Proximity.Near, -92, 0.5,-77),
                new(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 2, Proximity.Near, -93, 0.5,-77),
                
                new(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 3, Proximity.Near, -55, 0.5,-77),
                new(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 3, Proximity.Near, -55, 0.5,-77),
                new(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 3, Proximity.Near, -58, 0.5,-77),
                
                new(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 4, Proximity.Near, -77, 0.5,-77),
            };
            var sourse = listBeacons.ToObservable();
            
            //act
            var filter= sourse.AverageFilter(
               TimeSpan.FromSeconds(1),
               rssiList => (int) rssiList.Average(r=>r)
               );
            var filteredBeacons =(await filter.ToTask()).ToList();
            
            //Assert
            filteredBeacons.Count.Should().Be(4);
            
        }
    }
}