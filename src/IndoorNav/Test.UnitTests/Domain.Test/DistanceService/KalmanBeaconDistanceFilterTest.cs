using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Domain.DistanceService.Filters;
using ApplicationCore.Domain.DistanceService.Model;
using Libs.Beacons.Models;
using Xunit;

namespace Test.Beacons.Domain.Test.DistanceService
{
    public class KalmanBeaconDistanceFilterTest
    {
        [Fact]
        public void FiltrateForOneBeacon_Test()
        {
            //data

            var beaconDistanceInputDatas = new List<double>()
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
                }
                .Select(dist => new BeaconDistance(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1), dist))
                .ToList();
            
            
            //arrange
            var q = 1.0;
            var r = 15;
            var covarinace = 0.1;
            var filter = new KalmanBeaconDistanceFilter(q, r, covarinace);
            
            //act
            var filtred=beaconDistanceInputDatas
                .Select(bd => filter.Filtrate(bd))
                .ToList();
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
            var filter = new KalmanBeaconDistanceFilter(q, r, covarinace);
            
            //act
            var filtred = new List<BeaconDistance>();
            await foreach (var bd in GenerateDatas())
            {
                var res = filter.Filtrate(bd);
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
            var filter = new KalmanBeaconDistanceFilter(q, r, covarinace, rottenTime);
            
            //act
            var filtred = new List<BeaconDistance>();
            await foreach (var bd in GenerateDatas())
            {
                var res = filter.Filtrate(bd);
                filtred.Add(res);
            }
        }
    }
}