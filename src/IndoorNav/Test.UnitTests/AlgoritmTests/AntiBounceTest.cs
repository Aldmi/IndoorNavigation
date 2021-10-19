using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Shared.Models;
using ApplicationCore.Shared.Services;
using FluentAssertions;
using Xunit;

namespace Test.Beacons.AlgoritmTests
{
    public class AntiBounceTest
    {
                
        #region TheoryData
        public static IEnumerable<object[]> Datas => new[]
        {
            new object[]
            {
                0,
                new[]
                {
                    new Point(1,1),
                    new Point(1,1),
                    new Point(1,1),
                    new Point(2,2),
                    new Point(3,3),
                    new Point(3,3),
                    new Point(3,3),
                    new Point(4,4),
                    new Point(5,5),
                    new Point(6,6),
                    new Point(6,6),
                }
            },
            new object[]
            {
                1,
                new[]
                {
                    new Point(1,1),
                    new Point(1,1),
                    new Point(1,1),
                    new Point(2,2),
                    new Point(3,3),
                    new Point(3,3),
                    new Point(3,3),
                    new Point(4,4),
                    new Point(5,5),
                    new Point(6,6),
                    new Point(6,6),
                }
            },
            new object[]
            {
                2,
                new[]
                {
                    new Point(1,1),
                    new Point(1,1),
                    new Point(1,1),
                    new Point(1,1),
                    new Point(1,1),
                    new Point(3,3),
                    new Point(3,3),
                    new Point(3,3),
                    new Point(3,3),
                    new Point(3,3),
                    new Point(6,6),
                }
            },
            new object[]
            {
                3,
                new[]
                {
                    new Point(1,1),
                    new Point(1,1),
                    new Point(1,1),
                    new Point(1,1),
                    new Point(1,1),
                    new Point(1,1),
                    new Point(3,3),
                    new Point(3,3),
                    new Point(3,3),
                    new Point(3,3),
                    new Point(3,3),
                }
            },
            new object[]
            {
                4,
                new[]
                {
                    new Point(1,1),
                    new Point(1,1),
                    new Point(1,1),
                    new Point(1,1),
                    new Point(1,1),
                    new Point(1,1),
                    new Point(1,1),
                    new Point(1,1),
                    new Point(1,1),
                    new Point(1,1),
                    new Point(1,1),
                }
            },
        };
        #endregion
        [Theory]
        [MemberData(nameof(Datas))]
        public void InvokeTest(int acceptedCount, Point[] expectedResult)
        {
            //arrange
            var service = new AntiBounce<Point>(acceptedCount);
            var inValues = new[]
            {
                new Point(1,1),
                new Point(1,1),
                new Point(1,1),
                
                new Point(2,2),
                
                new Point(3,3),
                new Point(3,3),
                new Point(3,3),
                
                new Point(4,4),
                
                new Point(5,5),
                
                new Point(6,6),
                new Point(6,6),
            };
            
            //act
            var res=inValues.Select(value => service.Invoke(value)).ToList();
            
            //assert
            for (var i = 0; i < res.Count; i++)
            {
                res[i].Should().Be(expectedResult[i]);
            }
        }
    }
}