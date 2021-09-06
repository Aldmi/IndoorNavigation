using System;
using System.Collections.Generic;
using ApplicationCore.Domain.CheckPointModel;
using ApplicationCore.Domain.DistanceService;
using ApplicationCore.Domain.DistanceService.Model;
using ApplicationCore.Domain.MovingService.DiscreteSteps.Model;
using ApplicationCore.Shared.DataStruct.GraphNotOriented;
using Libs.Beacons.Models;

namespace Test.Beacons.Domain.Test.Data
{
    public static class CheckPointGraphSampleOne
    {
        public static Graph<CheckPointBase> CreateSimpleGraph()
        {
             var graph = new Graph<CheckPointBase>();
            
            //Создание чекпоинтов
            var checkPoints = new List<CheckPointDs>()
            {
                new CheckPointDs(
                    new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1),
                    new CheckPointDescription("Вход", "Зона 1"),
                    new CoverageArea(2)),
                
                new CheckPointDs(
                    new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 2),
                    new CheckPointDescription("Коридор 1", "Зона 2"),
                    new CoverageArea(2)),
                
                new CheckPointDs(
                    new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 3),
                    new CheckPointDescription("К поездам", "Зона 3"),
                    new CoverageArea(2)),
                
                new CheckPointDs(
                    new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 4),
                    new CheckPointDescription("Выход в город", "Зона 4"),
                    new CoverageArea(2)),
                
                new CheckPointDs(
                    new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 5),
                    new CheckPointDescription("Коридор 2", "Зона 5"),
                    new CoverageArea(2)),
                
                new CheckPointDs(
                    new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 6),
                    new CheckPointDescription("Кассы", "Зона 6"),
                    new CoverageArea(2)),
                
                new CheckPointDs(
                    new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 7),
                    new CheckPointDescription("Столовая", "Зона 7"),
                    new CoverageArea(2)),
            };
            
            //добавление вершин
            graph.AddVertexList(checkPoints);
            
            //добавление ребер (Путей)
            graph.AddEdge(checkPoints[0], checkPoints[1], 22);
            graph.AddEdge(checkPoints[1], checkPoints[2], 30);
            graph.AddEdge(checkPoints[1], checkPoints[3], 40);
            graph.AddEdge(checkPoints[1], checkPoints[4], 100);
            graph.AddEdge(checkPoints[4], checkPoints[5], 85);
            graph.AddEdge(checkPoints[4], checkPoints[6], 200);
            
            return graph;
        }
        
        
        
        public static List<List<BeaconDistance>> Route0To6()
        {
             return new()
             {
                new()// Обнаружили в зоне 2
                {
                    new BeaconDistance(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1), 5),
                    new BeaconDistance(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 2), 1.5),
                    new BeaconDistance(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 3), 5),
                },
                new()// продолжаем находится в зоне 2
                {
                    new BeaconDistance(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1), 5),
                    new BeaconDistance(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 2), 1.8),
                    new BeaconDistance(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 3), 5),
                },
                
                new() //вышли из всех зон (стали посупать данные от зоне 5)
                {
                    new BeaconDistance(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 1), 8),
                    new BeaconDistance(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 2), 4),
                    new BeaconDistance(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 3), 7),
                    new BeaconDistance(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 5), 5),
                },
                new() //вышли из всех зон (ближе к зоне 5)
                {
                    new BeaconDistance(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 2), 8),
                    new BeaconDistance(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 5), 3),
                },
                
                new() //Пришли к зоне 5
                {
                    new BeaconDistance(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 2), 9),
                    new BeaconDistance(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 5), 1.6),
                },
                new() //Стоим в зоне 5
                {
                    new BeaconDistance(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 2), 9),
                    new BeaconDistance(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 5), 1.8),
                },
                
                new() //Вышли из зоны 5
                {
                    new BeaconDistance(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 5), 9),
                    new BeaconDistance(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 6), 3),
                    new BeaconDistance(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 7), 10),
                },
                
                new() // Пришли к зоне 6
                {
                    new BeaconDistance(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 5), 9),
                    new BeaconDistance(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 6), 1.2),
                    new BeaconDistance(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 7), 10),
                },
                new() // Стоим в зоне 6
                {
                    new BeaconDistance(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 5), 9),
                    new BeaconDistance(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 6), 1.8),
                    new BeaconDistance(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 7), 10),
                }
             };
        }
    }
}