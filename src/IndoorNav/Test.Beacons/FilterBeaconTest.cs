using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Libs.Beacons;
using Libs.Beacons.Models;
using Xunit;

namespace Test.Beacons
{
    public class FilterBeaconTest
    {
        [Fact]
        public async Task AverageFilterTest()
        {
            var listBeacons= new List<Beacon>
            {
                new(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1, Proximity.Near, -88, 0.5, -77),
                new(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 2, Proximity.Near, -90, 0.5,-77),
                new(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 3, Proximity.Near, -55, 0.5,-77),
                new(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 3, Proximity.Near, -56, 0.5,-77),
                new(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 3, Proximity.Near, -62, 0.5,-77),
            };
            var sourse = listBeacons.ToObservable();
            
           var filter= sourse
               .Buffer(TimeSpan.FromSeconds(1))
               .Select(beacons =>
                   beacons.GroupBy(b => b.GetHashCode())
                       .Select(group =>
                       {
                           var beaconsInGroup = group.ToList();
                           var averageRssi = (int)beaconsInGroup.Average(b => b.Rssi);
                           
                           var filtredBeacon= beaconsInGroup.First().CreateByBlank(averageRssi);
                           return filtredBeacon;
                       })
                       .ToList());
           
          var filteredBeacons =(await filter.ToTask()).ToList();
        }
    }
}