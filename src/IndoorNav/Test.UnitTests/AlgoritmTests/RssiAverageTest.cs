using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Shared.Algoritms;
using Xunit;

namespace Test.Beacons.AlgoritmTests
{
    public class RssiAverageTest
    {
        //private List<int> RssiBufer = new() {-52, -53, -55, -54, -70};
        private List<int> RssiBufer = new() {-72, -73, -75, -74, -58};
        
        /// <summary>
        /// Считать лутчше среднее Rssi 
        /// </summary>
        [Fact]
        public void CalcAverageRssi()
        {
           var averageRssi= RssiBufer.Average();
           var dist=Rssi2DistanceAlgoritm.CalculateDistance(-50, averageRssi);

           var distList = RssiBufer.Select(rssi => Rssi2DistanceAlgoritm.CalculateDistance(-50, rssi)).ToList();
           var dist2 = distList.Average();
        }
    }
}