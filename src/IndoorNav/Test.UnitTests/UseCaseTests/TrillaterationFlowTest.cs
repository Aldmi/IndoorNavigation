using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using FluentAssertions;
using Libs.Beacons;
using Libs.Beacons.Flows;
using Libs.Beacons.Models;
using Test.Beacons.UseCaseTests.Data;
using UseCase.Trilateration.Flow;
using UseCase.Trilateration.Services;
using Xunit;


namespace Test.Beacons.UseCaseTests
{

    public class TrillaterationFlowTest
    {
        [Fact]
        public async Task ManagedScanFlowTest()
        {
            //Arrange
            var options = FourBeaconInRoom.CreateOption();
            var sphereFactory= new SphereFactory(Algoritm.CalculateDistance, options);
            var sourse = FourBeaconInRoom.CreateFlowImmediatly();
            var whiteList = options.Select(o => o.BeaconId).ToList();
            
             //act
             var spheres= sourse.ManagedScanFlow(whiteList, TimeSpan.FromSeconds(0.1), sphereFactory);
             var filteredBeacons =(await spheres.ToTask()).ToList();
            
             //Assert
             filteredBeacons.Count.Should().Be(4);
        }
    }
}