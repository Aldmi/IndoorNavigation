using System;
using System.Collections.Generic;using System.Linq;
using ApplicationCore.Domain.DiscreteSteps.Model;
using ApplicationCore.Domain.Distance;
using ApplicationCore.Domain.Navigation;
using ApplicationCore.Domain.Navigation.Model;
using ApplicationCore.Shared;
using ApplicationCore.Shared.DataStruct.GraphNotOriented;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.DiscreteSteps
{
    /// <summary>
    /// Сервис для работы с не направленным графом контрольных точек.
    /// </summary>
    public class GraphMovingCalculator : IMovingCalculator
    {
        private readonly Graph<CheckPoint> _graph;
        public GraphMovingCalculator(Graph<CheckPoint> graph)
        {
            _graph = graph;
        }
        
        /// <summary>
        /// Узел графа в котором мы находимся
        /// </summary>
        public Vertex<CheckPoint>? CurrentVertex { get; private set; }
        public bool CurrentVertexIsSet => CurrentVertex != null;
        public Guid SharedUuid => _graph.Vertices[0].Value.BeaconId.Uuid;
        
        
        /// <summary>
        /// Вычислить перемещение по узлам графа, анализируя входные данные.
        /// </summary>
        public Moving CalculateMove(IEnumerable<BeaconDistanceModel> inputDataList)
        {
            Moving moving;
            if (!CurrentVertexIsSet)
            {
                var vertex= FindFirstCurrentVertex(inputDataList);
                if (vertex != null)
                {
                    CurrentVertex = vertex;
                    moving= Moving.InitSegment(vertex.Value);                                   //Выставить первый раз Стартовый сегмент.  
                }
                else
                {
                    moving= Moving.UnknownSegment();                                            //Стартовый сегмент не найден.
                }
            }
            else
            {
                var vertex= FindAmongNeighborsOfCurrentVertex(inputDataList);
                if (vertex != null)
                {
                    if (vertex == CurrentVertex)
                    {
                        moving = Moving.StartSegment(CurrentVertex!.Value);                      //Стоим около стартового сегмента.
                    }
                    else
                    {
                        moving = Moving.CompleteSegment(CurrentVertex!.Value, vertex.Value);    //Однократно выставили завершающий сегмент.
                        CurrentVertex = vertex;
                    }
                }
                else
                {
                    moving = Moving.GoToEnd(CurrentVertex!.Value);                              //Начали движение от стартового сегмента.
                }
            }
            
            //moving = Moving.CompleteSegment(CurrentVertex!.Value, CurrentVertex!.Value);    //Debug
            return moving;
        }
        
        
        /// <summary>
        /// Сбросить текщее положение в графе.
        /// </summary>
        public void Reset() => CurrentVertex = null;
        
        
        /// <summary>
        /// Найти текущую вершину графа в первый раз.
        /// </summary>
        private Vertex<CheckPoint>? FindFirstCurrentVertex(IEnumerable<BeaconDistanceModel> inputDataList)
        {
            foreach (var inputData in inputDataList)
            {
                var vertex = _graph.FindVertex(node => node.Value.GetZone(inputData) == Zone.In);
                if (vertex != null)
                    return vertex;
            }
            return null;
        }
        
        
        /// <summary>
        /// Найти среди соседей текущей врешины.
        /// </summary>
        private Vertex<CheckPoint>? FindAmongNeighborsOfCurrentVertex(IEnumerable<BeaconDistanceModel> inputDataList)
        {
            foreach (var inputData in inputDataList)
            {
                var vertex = _graph.FindVertexAmongNeighbors(CurrentVertex!, node => node.Value.GetZone(inputData) == Zone.In);
                if (vertex != null)
                    return vertex;
            }
            return null;
        }
    }
}