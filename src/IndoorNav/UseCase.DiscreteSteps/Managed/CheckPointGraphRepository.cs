using System;
using System.Collections.Generic;
using ApplicationCore.Domain.CheckPointModel;
using ApplicationCore.Domain.CheckPointModel.DiscreteSteps;
using ApplicationCore.Shared.DataStruct.GraphNotOriented;
using Libs.Beacons.Models;

namespace UseCase.DiscreteSteps.Managed
{
    public class CheckPointGraphRepository : ICheckPointGraphRepository  //TODO: вынести в Infrastructure
    {
       // // моя квартира (лестничная площадка)
       //  public Graph<CheckPointBase> GetGraph()
       //  {
       //       var graph = new Graph<CheckPointBase>();
       //      
       //      //Создание чекпоинтов
       //      var checkPoints = new List<CheckPointDs>
       //      {
       //          new CheckPointDs(
       //              new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"),  65438, 43487),
       //              new CheckPointDescription("Лифт", "лифтовой хол"),
       //              new CoverageArea(1.2)),
       //          
       //          new CheckPointDs(
       //              new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 35144, 19824),
       //              new CheckPointDescription("Коридор", "Коридор"),
       //              new CoverageArea(1.2)),
       //          
       //          new CheckPointDs(
       //              new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 48943, 20570),
       //              new CheckPointDescription("Кв.'102'", "Конец коридора Кв '102'"),
       //              new CoverageArea(1.2)),
       //          
       //          new CheckPointDs(
       //              new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 56954, 34501),
       //              new CheckPointDescription("Кв.'98'", "Конец коридора Кв '98'"),
       //              new CoverageArea(1.2)),
       //      };
       //      
       //      //добавление вершин
       //      graph.AddVertexList(checkPoints);
       //      
       //      //добавление ребер (Путей)
       //      graph.AddEdge(checkPoints[1], checkPoints[0], 450); //Коридор-лифт
       //      graph.AddEdge(checkPoints[1], checkPoints[2], 700); //Коридор-Кв.102
       //      graph.AddEdge(checkPoints[1], checkPoints[3], 700); //Коридор-Кв.103
       //      return graph;
       //  }
        
        
       // офис
       public Graph<CheckPointBase> GetGraph()
       {
           var graph = new Graph<CheckPointBase>();
            
           //Создание чекпоинтов
           var checkPoints = new List<CheckPointDs>
           {
               new CheckPointDs(new CheckPointDescription("Кабинет 1", "Кабинет 1 - выход в холл"),
                   new BeaconCheckPointItem(
                       new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"),  65438, 43487),
                       new CoverageArea(1.5))
                   ),
               
               new CheckPointDs(new CheckPointDescription("Кабинет 2", "Кабинет 2"),
                   new BeaconCheckPointItem(
                       new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 35144, 19824),
                       new CoverageArea(0.8))
               ),
               
               new CheckPointDs(new CheckPointDescription("Кабинет 3", "Кабинет 3"),
                   new BeaconCheckPointItem(
                       new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 56400, 31127),
                       new CoverageArea(0.8))
               ),
               
               new CheckPointDs(new CheckPointDescription("Кабинет 4", "Кабинет 4"),
                   new BeaconCheckPointItem(
                       new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 56954, 34501),
                       new CoverageArea(1.5))
               ),
               
               new CheckPointDs(new CheckPointDescription("Сан узел", "Сан узел"),
                   new BeaconCheckPointItem(
                       new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 58734, 51137),
                       new CoverageArea(1.5))
               ),
           };
            
           //добавление вершин
           graph.AddVertexList(checkPoints);
            
           //добавление ребер (Путей)
           graph.AddEdge(checkPoints[0], checkPoints[1], 3000+3000+2700); //Кабинет1-Кабинет2
           graph.AddEdge(checkPoints[1], checkPoints[2], 2641); //Кабинет2-Кабинет3
           graph.AddEdge(checkPoints[2], checkPoints[3], 5449); //Кабинет3-Кабинет4
           graph.AddEdge(checkPoints[3], checkPoints[4], 7314); //Кабинет4-Сан узел
           return graph;
       }

       public Guid GetSharedUuid()
       {
           return Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e");
       }
    }
}