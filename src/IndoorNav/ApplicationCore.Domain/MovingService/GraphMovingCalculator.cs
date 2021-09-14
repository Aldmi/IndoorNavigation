using System;
using System.Collections.Generic;
using ApplicationCore.Domain.CheckPointModel;
using ApplicationCore.Domain.DistanceService.Model;
using ApplicationCore.Domain.MovingService.Model;
using ApplicationCore.Shared;
using ApplicationCore.Shared.DataStruct.GraphNotOriented;

namespace ApplicationCore.Domain.MovingService
{
    /// <summary>
    /// Сервис для работы с не направленным графом контрольных точек.
    /// </summary>
    public class GraphMovingCalculator : IGraphMovingCalculator
    {
        private readonly Graph<CheckPointBase> _graph;
        private Vertex<CheckPointBase>? _сurrentVertex;
        public GraphMovingCalculator(Guid sharedUuid, Graph<CheckPointBase> graph)
        {
            SharedUuid = sharedUuid;
            _graph = graph;
        }

        /// <summary>
        /// Узел графа в котором мы находимся
        /// </summary>
        public CheckPointBase? CurrentCheckPoint => _сurrentVertex?.Value;
        public bool CurrentVertexIsSet => _сurrentVertex != null;
        
        /// <summary>
        /// 
        /// </summary>
        public Guid SharedUuid { get; }


        /// <summary>
        /// Вычислить перемещение по узлам графа, анализируя входные данные.
        /// </summary>
        public Moving CalculateMove(IEnumerable<BeaconDistance> inputDataList)
        {
            Moving moving;
            if (!CurrentVertexIsSet)
            {
                var vertex= FindFirstCurrentVertex(inputDataList);
                if (vertex != null)
                {
                    _сurrentVertex = vertex;
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
                    if (vertex == _сurrentVertex)
                    {
                        moving = Moving.StartSegment(_сurrentVertex!.Value);                      //Стоим около стартового сегмента.
                    }
                    else
                    {
                        moving = Moving.CompleteSegment(_сurrentVertex!.Value, vertex.Value);    //Однократно выставили завершающий сегмент.
                        _сurrentVertex = vertex;
                    }
                }
                else
                {
                    moving = Moving.GoToEnd(_сurrentVertex!.Value);                              //Начали движение от стартового сегмента.
                }
            }
            
            //moving = Moving.CompleteSegment(CurrentVertex!.Value, CurrentVertex!.Value);    //Debug
            return moving;
        }
        
        
        /// <summary>
        /// Сбросить текщее положение в графе.
        /// </summary>
        public void Reset() => _сurrentVertex = null;
        
        
        /// <summary>
        /// Найти текущую вершину графа в первый раз.
        /// </summary>
        private Vertex<CheckPointBase>? FindFirstCurrentVertex(IEnumerable<BeaconDistance> distances)
        {
            var vertex = _graph.FindVertex(v => v.Value.GetZone(distances) == Zone.In);
            return vertex;
        }
        
        
        /// <summary>
        /// Найти среди соседей текущей врешины.
        /// </summary>
        private Vertex<CheckPointBase>? FindAmongNeighborsOfCurrentVertex(IEnumerable<BeaconDistance> distances)
        {
            var vertex = _graph.FindVertexAmongNeighbors(_сurrentVertex!, v => v.Value.GetZone(distances) == Zone.In);
            return vertex;
        }
    }
}