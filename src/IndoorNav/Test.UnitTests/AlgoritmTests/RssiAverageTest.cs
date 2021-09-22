using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Domain.DistanceService;
using Xunit;

namespace Test.Beacons.AlgoritmTests
{
    public class RssiAverageTest
    {
        //private List<int> RssiBufer = new() {-52, -53, -55, -54, -70};
        private List<int> RssiBufer = new() {-62, -63, -65, -74, -58};
        
        /// <summary>
        /// Считать лутчше среднее Rssi 
        /// </summary>
        [Fact]
        public void CalcAverageRssi()
        {
           var averageRssi= (int)Math.Round(RssiBufer.Average());
           var dist=Rssi2DistanceConverter.CalculateDistance(-50, averageRssi);

           var distList = RssiBufer.Select(rssi => Rssi2DistanceConverter.CalculateDistance(-50, rssi)).ToList();
           var dist2 = distList.Average();
        }
    }
}