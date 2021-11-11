using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Domain.DistanceService.Model;
using ApplicationCore.Shared.Filters.Kalman;
using Libs.Beacons.Models;
using Xunit;
using Xunit.Abstractions;

namespace Test.Beacons.KalmanFiltersTests
{
    public class KalmanBeaconDistanceFilterTest
    {
        private readonly ITestOutputHelper _testOutputHelper;
        public KalmanBeaconDistanceFilterTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }
        
        
        [Fact]
        public void FiltrateForOneBeacon_Test()
        {
            //data
            var beaconDistances = new List<double>()
                {
                    2.5,
                    2.8,
                    2.6,
                    2.6,
                    1.9,
                    3.5,
                    5.2,
                    4.2,
                    5.1,
                    5.0,
                    2.7,
                    1.5,
                    2.9,
                    
                    9.3,
                    9.0,
                    8.3,
                    4.3,
                    1.1,
                    1.1,
                    1.0
                }
                .Select(dist => new BeaconDistance(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1), dist))
                .ToList();
            
            //arrange
            var q = 1.5;
            var r = 10;
            var covarinace = 2.0;
            var filter = new Kalman1DFilterWrapper(q, r, covarinace);
            
            //act
            var filtred=beaconDistances
                .Select(bd => filter.Filtrate(bd.BeaconId, bd.Distance))
                .ToList();
            
            //assert
            for (int i = 0; i < beaconDistances.Count; i++)
            {
                var bd = beaconDistances[i];
                var filtredData = filtred[i];
                _testOutputHelper.WriteLine($"{bd.Distance:f1}\t  {filtredData:f1}");
            }
        }
        
        
        [Fact]
        public async Task FiltrateForOneBeacon_IsRotten_False_Test()
        {
            //data
            var generateTime = TimeSpan.FromSeconds(0.5);
            async IAsyncEnumerable<BeaconDistance> GenerateDatas()
            {
                yield return new(
                    new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1),
                    1.0);
                
                await Task.Delay(generateTime);
                
                yield return new(
                    new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1),
                    2.0);
                
                await Task.Delay(generateTime);
                
                yield return new(
                    new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1),
                    5.0);
                
                await Task.Delay(generateTime);
                
                yield return new(
                    new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1),
                    25.0);
                
                await Task.Delay(generateTime);
                
                yield return new(
                    new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1),
                    7.0);
                
                await Task.Delay(generateTime);
                
                yield return new(
                    new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1),
                    10.0);
            } 
            
            //arrange
            var q = 1.0;
            var r = 15;
            var covarinace = 0.1;
            var filter = new Kalman1DFilterWrapper(q, r, covarinace);
            
            //act
            var filtred = new List<double>();
            await foreach (var bd in GenerateDatas())
            {
                var res = filter.Filtrate(bd.BeaconId, bd.Distance);
                filtred.Add(res);
            }
        }
        
        
        [Fact]
        public async Task FiltrateForOneBeacon_IsRotten_true_Test()
        {
            //data
            var generateTime = TimeSpan.FromSeconds(0.5);
            var pauseTime = TimeSpan.FromSeconds(2.5);
            async IAsyncEnumerable<BeaconDistance> GenerateDatas()
            {
                yield return new(
                    new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1),
                    1.0);
                await Task.Delay(generateTime);
                yield return new(
                    new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1),
                    2.0);
                await Task.Delay(generateTime);
                yield return new(
                    new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1),
                    5.0);
                await Task.Delay(generateTime);
                yield return new(
                    new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1),
                    25.0);
                await Task.Delay(generateTime);
                yield return new(
                    new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1),
                    7.0);
                await Task.Delay(generateTime);
                yield return new(
                    new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1),
                    10.0);
                
                //--------------------pause
                await Task.Delay(pauseTime);
                yield return new(
                    new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1),
                    1.0);
                await Task.Delay(generateTime);
                yield return new(
                    new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1),
                    2.0);
                await Task.Delay(generateTime);
                yield return new(
                    new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1),
                    3.0);
            } 
            
            //arrange
            var q = 1.0;
            var r = 15;
            var covarinace = 0.1;
            var rottenTime = TimeSpan.FromSeconds(2);
            var filter = new Kalman1DFilterWrapper(q, r, covarinace, rottenTime);
            
            //act
            var filtred = new List<double>();
            await foreach (var bd in GenerateDatas())
            {
                var res = filter.Filtrate(bd.BeaconId, bd.Distance);
                filtred.Add(res);
            }
        }
    }
}