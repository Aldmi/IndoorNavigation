using System;
using System.Collections.Generic;
using ApplicationCore.Domain.DiscreteSteps;
using ApplicationCore.Domain.DiscreteSteps.Model;
using ApplicationCore.Shared.DataStruct;
using ApplicationCore.Shared.DataStruct.GraphNotOriented;
using ApplicationCore.Shared.DataStruct.Tree;
using Libs.Beacons.Models;

namespace UseCase.DiscreteSteps.Managed
{
    public class CheckPointGraphRepository : ICheckPointGraphRepository  //TODO: вынести в Infrastructure
    {
        public Graph<CheckPoint> GetGraph()
        {
             var graph = new Graph<CheckPoint>();
            
            //Создание чекпоинтов
            var checkPoints = new List<CheckPoint>
            {
                new CheckPoint(
                    new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"),  65438, 43487),
                    new CheckPointDescription("Лифт", "лифтовой хол"),
                    new CoverageArea(1.2)),
                
                new CheckPoint(
                    new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 35144, 19824),
                    new CheckPointDescription("Коридор", "Коридор"),
                    new CoverageArea(1.2)),
                
                new CheckPoint(
                    new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 48943, 20570),
                    new CheckPointDescription("Кв.'102'", "Конец коридора Кв '102'"),
                    new CoverageArea(1.2)),
                
                new CheckPoint(
                    new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 56954, 34501),
                    new CheckPointDescription("Кв.'98'", "Конец коридора Кв '98'"),
                    new CoverageArea(1.2)),
            };
            
            //добавление вершин
            graph.AddVertexList(checkPoints);
            
            //добавление ребер (Путей)
            graph.AddEdge(checkPoints[1], checkPoints[0], 450); //Коридор-лифт
            graph.AddEdge(checkPoints[1], checkPoints[2], 700); //Коридор-Кв.102
            graph.AddEdge(checkPoints[1], checkPoints[3], 700); //Коридор-Кв.103
            return graph;
        }
    }
}