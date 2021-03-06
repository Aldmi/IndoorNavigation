using System;
using System.Linq;
using ApplicationCore.Domain.CheckPointModel;
using ApplicationCore.Domain.RouteTrackingService.Model;
using ApplicationCore.Shared.DataStruct.GraphNotOriented;
using ApplicationCore.Shared.DataStruct.GraphNotOriented.DijkstraAlgoritm;

namespace ApplicationCore.Domain.RouteTrackingService
{
    public class RouteBuilder : IRouteBuilder
    {
        private readonly Dijkstra<CheckPointBase> _dijkstra;
        public RouteBuilder(Graph<CheckPointBase> graph)
        {
            _dijkstra = new Dijkstra<CheckPointBase>(graph);
        }
        
        
        public Route Build(string name, CheckPointBase startCh, CheckPointBase endCh)
        {
            var listCheckPoints= _dijkstra.FindShortestPath(startCh, endCh)
                .Select(vertex=>vertex.Value)
                .ToList();

            return new Route(name, listCheckPoints);
        }

        public IObservable<Route> BuildRouteRx { get; }
    }
}