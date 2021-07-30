using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Test.Beacons
{
    public class AlgoritmTest
    {
        [Fact]
        public void AverageSimpleTest()
        {
            var hyh = Math.Round(6.48);
            
            //act
            List<int> nums = Enumerable.Range(1, 5).ToList();
            var func= new Func<List<int>, int>(rssiList => (int) Math.Round(rssiList.Average(r => r)));
                
            //assert
            var average= func(nums);

            average.Should().Be(10);
        }
    }
}