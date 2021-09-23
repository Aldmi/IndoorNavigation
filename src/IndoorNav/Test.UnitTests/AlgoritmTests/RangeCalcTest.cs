using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Domain.DistanceService;
using ApplicationCore.Domain.DistanceService.Helpers;
using ApplicationCore.Shared;
using FluentAssertions;
using UseCase.Trilateration.Flow;
using Xunit;

namespace Test.Beacons.AlgoritmTests
{
    public class RangeCalcTest
    {
        [Fact]
        public void RangeCalc_Tx_Minus59()
        {
            //arrange
            var rssiList=  Enumerable
                .Range(50, 40)
                .Select(i => i * -1)
                .ToList();
            
            //act
            var rangeDict= rssiList
                .ToDictionary(rssi=>rssi, rssi=>RssiHelpers.CalculateDistance(-59, rssi));
            
            //TODO: записать значения
            //assert
            rangeDict[-50].Should().Be(0.20);
            rangeDict[-51].Should().Be(0.20);
            rangeDict[-52].Should().Be(0.30);
            rangeDict[-53].Should().Be(0.30);
            rangeDict[-54].Should().Be(0.40);
            rangeDict[-55].Should().Be(0.50);
            rangeDict[-56].Should().Be(0.60);
            rangeDict[-57].Should().Be(0.70);
            rangeDict[-58].Should().Be(0.80);
            
            rangeDict[-59].Should().Be(1.00);
            rangeDict[-60].Should().Be(1.10);
            rangeDict[-61].Should().Be(1.30);
            rangeDict[-62].Should().Be(1.40);
            rangeDict[-63].Should().Be(1.60);
            
            rangeDict[-64].Should().Be(1.80);
            rangeDict[-65].Should().Be(2.00);
            
            rangeDict[-66].Should().Be(2.20);
            rangeDict[-67].Should().Be(2.50);
            rangeDict[-68].Should().Be(2.80);
            
            rangeDict[-69].Should().Be(3.10);
            rangeDict[-70].Should().Be(3.50);
            
            rangeDict[-71].Should().Be(3.90);
            rangeDict[-72].Should().Be(4.30);
            
            rangeDict[-73].Should().Be(4.80);
            rangeDict[-74].Should().Be(5.30);
            
            rangeDict[-75].Should().Be(5.80);
            rangeDict[-76].Should().Be(6.40);
            
            rangeDict[-77].Should().Be(7.10);
            rangeDict[-78].Should().Be(7.90);
            
            rangeDict[-79].Should().Be(8.70);
            rangeDict[-80].Should().Be(9.50);
            rangeDict[-81].Should().Be(10.50);
            rangeDict[-82].Should().Be(11.50);
            rangeDict[-83].Should().Be(12.60);
            rangeDict[-84].Should().Be(13.80);
            rangeDict[-85].Should().Be(15.10);
            rangeDict[-86].Should().Be(16.50);
            rangeDict[-87].Should().Be(18.10);
            rangeDict[-88].Should().Be(19.70);
            rangeDict[-89].Should().Be(21.50);
        }



        [Fact]
        public void xxxxx()
        {
            var dist = RssiHelpers.CalculateDistance(-59, -86);
            
        }
        
    }
}