using System;
using System.Collections.Generic;
using ApplicationCore.Domain.DiscreteSteps;
using ApplicationCore.Shared.DataStruct;
using Libs.Beacons.Models;
using Xunit;

namespace Test.Beacons.Domain.Test
{
    public class CheckPointGraphTests
    {
        [Fact]
        public void CalculateMoveTest()
        {
            //arrange
            var root = new TreeNode<CheckPoint>(new CheckPoint(
                new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1),
                new CheckPointDescription("Вход", "Вход на вокзал"),
                new CoverageArea(2))
            );
            
            var k1= root.AddChild(new CheckPoint(
                new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 2),
                new CheckPointDescription("Коридор 1", "Главный коридор"),
                new CoverageArea(2)));
            
            k1.AddChildren(new CheckPoint(
                new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 3),
                new CheckPointDescription("К поездам", "Выход на преррон"),
                new CoverageArea(2)), new CheckPoint(
                new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 4),
                new CheckPointDescription("Выход в город", "Выход из вокзала в город"),
                new CoverageArea(2)));
            
            var k2=k1.AddChild(new CheckPoint(
                new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 5),
                new CheckPointDescription("Коридор 2", "Коридор 2"),
                new CoverageArea(2)));
            
            k2.AddChildren(new CheckPoint(
                new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 6),
                new CheckPointDescription("Кассы", "Кассы"),
                new CoverageArea(2)), new CheckPoint(
                new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 7),
                new CheckPointDescription("Столовая", "Столовая"),
                new CoverageArea(2)));
            
            var graph = new CheckPointGraph(root);

            var inDataListStep1 = new List<InputData>
            {
                new InputData(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1), 5),
                new InputData(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 2), 1.5),
                new InputData(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 3), 5),
            };
            
            var inDataListStep2 = new List<InputData>
            {
                new InputData(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1), 1.6),
                new InputData(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 2), 3),
                new InputData(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 3), 5),
            };
            
            //act
           
            var res1 = graph.CalculateMove(inDataListStep1);
            // Встали на "Коридор 1"
            
            var res2 = graph.CalculateMove(inDataListStep1);
            
       
            
            
            //asser
        }
    }
}