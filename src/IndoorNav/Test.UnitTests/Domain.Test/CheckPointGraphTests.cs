using System;
using System.Collections.Generic;
using ApplicationCore.Domain.DiscreteSteps;
using ApplicationCore.Domain.Services;
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
            
            
            var route2Exitg = new List<List<BeaconDistanceModel>>
            {
                new()
                {
                    new BeaconDistanceModel(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1), 5),
                    new BeaconDistanceModel(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 2), 1.5),
                    new BeaconDistanceModel(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 3), 5),
                },
                new()
                {
                    new BeaconDistanceModel(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1), 5),
                    new BeaconDistanceModel(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 2), 1.6),
                    new BeaconDistanceModel(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 3), 5),
                },
                new()
                {
                    new BeaconDistanceModel(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1), 3),
                    new BeaconDistanceModel(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 2), 4),
                    new BeaconDistanceModel(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 3), 7),
                },
                new()
                {
                    new BeaconDistanceModel(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1), 1.5),
                    new BeaconDistanceModel(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 2), 6),
                    new BeaconDistanceModel(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 3), 10),
                }
            };
            
            var route2Kassy = new List<List<BeaconDistanceModel>>
            {
                new()// Обнаружили в зоне 2
                {
                    new BeaconDistanceModel(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1), 5),
                    new BeaconDistanceModel(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 2), 1.5),
                    new BeaconDistanceModel(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 3), 5),
                },
                new()// продолжаем находится в зоне 2
                {
                    new BeaconDistanceModel(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1), 5),
                    new BeaconDistanceModel(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 2), 1.8),
                    new BeaconDistanceModel(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 3), 5),
                },
                new() //вышли из всех зон (стали посупрать данные от зоне 5)
                {
                    new BeaconDistanceModel(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1), 8),
                    new BeaconDistanceModel(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 2), 4),
                    new BeaconDistanceModel(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 3), 7),
                    new BeaconDistanceModel(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 5), 5),
                },
                new() //вышли из всех зон (ближе к зоне 5)
                {
                    new BeaconDistanceModel(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 2), 8),
                    new BeaconDistanceModel(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 5), 3),
                },
                new() //Пришли к зоне 5
                {
                    new BeaconDistanceModel(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 2), 9),
                    new BeaconDistanceModel(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 5), 1.6),
                },
                
                new() //Вышли из зоны 5
                {
                    new BeaconDistanceModel(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 5), 9),
                    new BeaconDistanceModel(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 6), 3),
                    new BeaconDistanceModel(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 7), 10),
                },
                new() // Пришли к зоне 6
                {
                    new BeaconDistanceModel(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 5), 9),
                    new BeaconDistanceModel(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 6), 1.2),
                    new BeaconDistanceModel(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 7), 10),
                }
            };
                
            
             //act
            // // Встали на "Коридор 1"
            // var res1 = graph.CalculateMove(route2Exitg[0]);
            // // Продолжаем стоять в зоне датчика "Коридор 1"
            // var res2 = graph.CalculateMove(route2Exitg[1]);
            // // вышли из зоны датчика (непонятно куда идем)
            // var res3 = graph.CalculateMove(route2Exitg[2]);
            // // Пришли на выход
            // var res4 = graph.CalculateMove(route2Exitg[3]);
            
            
            // Встали на "Коридор 1"
            var res1 = graph.CalculateMove(route2Kassy[0]);
            // 
            var res2 = graph.CalculateMove(route2Kassy[1]);
            // 
            var res3 = graph.CalculateMove(route2Kassy[2]);
            //
            var res4 = graph.CalculateMove(route2Kassy[3]);
            //
            var res5 = graph.CalculateMove(route2Kassy[4]);
            // 
            var res6 = graph.CalculateMove(route2Kassy[5]);
            // 
            var res7 = graph.CalculateMove(route2Kassy[6]);

            
       
            
            
            //asser
        }
    }
}