using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Domain.DistanceService;
using ApplicationCore.Domain.DistanceService.Helpers;
using Xunit;

namespace Test.Beacons.AlgoritmTests
{
    public class RssiAverageTest
    {
        private List<int> RssiBufer = new() {-52, -53, -55, -54, -70};
        //private List<int> RssiBufer = new() {-62, -63, -65, -74, -58};
        
        /// <summary>
        /// Считать лутчше среднее Rssi 
        /// </summary>
        [Fact]
        public void CalcAverageRssi()
        {
           var averageRssi= (int)Math.Round(RssiBufer.Average());
           var dist=RssiHelpers.CalculateDistance(-50, averageRssi);
           
           var averageRssiDouble= Math.Round(RssiBufer.Average(), 2);
           var dist2=RssiHelpers.CalculateDistance(-50, averageRssiDouble);

           var distList = RssiBufer.Select(rssi => RssiHelpers.CalculateDistance(-50, rssi)).ToList();
           var dist3 = distList.Select(res=> res.Value).Average();
        }
    }
}