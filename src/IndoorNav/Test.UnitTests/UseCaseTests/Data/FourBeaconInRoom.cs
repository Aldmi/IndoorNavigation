using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using ApplicationCore.Domain;
using ApplicationCore.Domain.Options;
using ApplicationCore.Domain.Shared;
using Libs.Beacons;
using Libs.Beacons.Models;
using UseCase.Trilateration.Flow;

namespace Test.Beacons.UseCaseTests.Data
{
    public static class FourBeaconInRoom
    {
        /// <summary>
        /// Создать поток мгновенно (на базе List)
        /// </summary>
        public static IObservable<Beacon> CreateFlowImmediatly()
        {
            var options = CreateOption();
            var beaconId_0 = options[0].BeaconId;
            var beaconId_1 = options[1].BeaconId;
            var beaconId_2 = options[2].BeaconId;
            var beaconId_3 = options[3].BeaconId;
            
            var listBeacons= new List<Beacon>
            {
                new(beaconId_0.Uuid, beaconId_0.Major, beaconId_0.Minor, Proximity.Near, -77, 0.5, 0),
                new(beaconId_0.Uuid, beaconId_0.Major, beaconId_0.Minor, Proximity.Near, -77, 0.5, 0),
                new(beaconId_0.Uuid, beaconId_0.Major, beaconId_0.Minor, Proximity.Near, -77, 0.5, 0),
                
                new(beaconId_1.Uuid, beaconId_1.Major, beaconId_1.Minor, Proximity.Near, -77, 0.5, 0),
                new(beaconId_1.Uuid, beaconId_1.Major, beaconId_1.Minor, Proximity.Near, -77, 0.5, 0),
                new(beaconId_1.Uuid, beaconId_1.Major, beaconId_1.Minor, Proximity.Near, -77, 0.5, 0),
                
                new(beaconId_2.Uuid, beaconId_2.Major, beaconId_2.Minor, Proximity.Near, -77, 0.5, 0),
                new(beaconId_2.Uuid, beaconId_2.Major, beaconId_2.Minor, Proximity.Near, -77, 0.5, 0),
                new(beaconId_2.Uuid, beaconId_2.Major, beaconId_2.Minor, Proximity.Near, -77, 0.5, 0),
                
                new(beaconId_3.Uuid, beaconId_3.Major, beaconId_3.Minor, Proximity.Near, -77, 0.5, 0),
                new(beaconId_3.Uuid, beaconId_3.Major, beaconId_3.Minor, Proximity.Near, -77, 0.5, 0),
                new(beaconId_3.Uuid, beaconId_3.Major, beaconId_3.Minor, Proximity.Near, -77, 0.5, 0),
            };
            var sourse = listBeacons.ToObservable();
            return sourse;
        }


        public static IList<BeaconOption> CreateOption()
        {
            return new List<BeaconOption>
            {
                new BeaconOption(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1), 1, -77,
                    new Point(1, 1)),
                new BeaconOption(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 2), 1, -77,
                    new Point(6, 1)),
                new BeaconOption(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 3), 1, -77,
                    new Point(1, 7)),
                new BeaconOption(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 4), 1, -77,
                    new Point(7, 6)),
            };
        }


        public static SphereFactory CreateSphereFactory()
        {
            return new SphereFactory(Algoritm.CalculateDistance, CreateOption());
        }
    }
}