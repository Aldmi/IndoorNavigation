using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using UseCase.Trilateration.Flow;
using Xunit;

namespace Test.Beacons.AlgoritmTests
{
    public class RangeCalcTest
    {
        [Fact]
        public void RangeCalc()
        {
            //arrange
            var rssiList=  Enumerable
                .Range(77, 30)
                .Select(i => i * -1)
                .ToList();
            
            //act
            var rangeDict= rssiList
                .ToDictionary(rssi=>rssi, rssi=>Math.Round(Algoritm.CalculateDistance(-77, rssi), 2));

            var r = Math.Round(3.690009,2, MidpointRounding.ToPositiveInfinity);
            
            //assert
            rangeDict[-77].Should().Be(1.01);
            rangeDict[-78].Should().Be(1.10);
            rangeDict[-79].Should().Be(1.21);
            rangeDict[-80].Should().Be(1.32);
            rangeDict[-81].Should().Be(1.44);
            rangeDict[-82].Should().Be(1.57);
            rangeDict[-83].Should().Be(1.72);
            rangeDict[-84].Should().Be(1.87);
            rangeDict[-85].Should().Be(2.04);
            rangeDict[-86].Should().Be(2.22);
            
            rangeDict[-87].Should().Be(2.42);
            rangeDict[-88].Should().Be(2.63);
            rangeDict[-89].Should().Be(2.86);
            rangeDict[-90].Should().Be(3.11);
            rangeDict[-91].Should().Be(3.37);
            
            rangeDict[-92].Should().Be(3.66);
            rangeDict[-93].Should().Be(3.97);
            rangeDict[-94].Should().Be(4.30);
            rangeDict[-95].Should().Be(4.66);
            rangeDict[-96].Should().Be(5.04);
            rangeDict[-97].Should().Be(5.45);
            rangeDict[-98].Should().Be(5.89);
            rangeDict[-99].Should().Be(6.36);
            rangeDict[-100].Should().Be(6.86);
            rangeDict[-101].Should().Be(7.40);
            rangeDict[-102].Should().Be(7.97);
            rangeDict[-103].Should().Be(8.59);
            rangeDict[-104].Should().Be(9.24);
            rangeDict[-105].Should().Be(9.94);
            rangeDict[-106].Should().Be(10.69);
        }
    }
}