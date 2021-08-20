using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Shared.DataStruct.GraphNotOriented.DijkstraAlgoritm
{
    /// <summary>
    /// Алгоритм Дейкстры
    /// </summary>
    public class Dijkstra<T> where T : IEquatable<T>
    {
        private readonly Graph<T> _graph;
        private List<VertexInfo<T>> _infos;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="graph">Граф</param>
        public Dijkstra(Graph<T> graph)
        {
            _graph = graph;
        }

        /// <summary>
        /// Инициализация информации
        /// </summary>
        private void InitInfo()
        {
            _infos = new List<VertexInfo<T>>();
            foreach (var v in _graph.Vertices)
            {
                _infos.Add(new VertexInfo<T>(v));
            }
        }

        /// <summary>
        /// Получение информации о вершине графа
        /// </summary>
        /// <param name="v">Вершина</param>
        /// <returns>Информация о вершине</returns>
        VertexInfo<T>? GetVertexInfo(Vertex<T> v) => 
            _infos.FirstOrDefault(i => i.Vertex.Equals(v));


        /// <summary>
        /// Поиск непосещенной вершины с минимальным значением суммы
        /// </summary>
        /// <returns>Информация о вершине</returns>
        public VertexInfo<T>? FindUnvisitedVertexWithMinSum()
        {
            var minValue = int.MaxValue;
            VertexInfo<T> minVertexInfo = null;
            foreach (var i in _infos)
            {
                if (i.IsUnvisited && i.EdgesWeightSum < minValue)
                {
                    minVertexInfo = i;
                    minValue = i.EdgesWeightSum;
                }
            }
            return minVertexInfo;
        }

        /// <summary>
        /// Поиск кратчайшего пути по данным вершин
        /// </summary>
        /// <param name="startValue">данные стартовой вершины</param>
        /// <param name="finishValue">данные финишной вершины</param>
        /// <returns>Кратчайший путь</returns>
        public IEnumerable<Vertex<T>> FindShortestPath(T startValue, T finishValue)
        {
            return FindShortestPath(_graph.FindVertex(startValue), _graph.FindVertex(finishValue));
        }

        /// <summary>
        /// Поиск кратчайшего пути по вершинам
        /// </summary>
        /// <param name="startVertex">Стартовая вершина</param>
        /// <param name="finishVertex">Финишная вершина</param>
        /// <returns>Кратчайший путь</returns>
        public IEnumerable<Vertex<T>> FindShortestPath(Vertex<T>? startVertex, Vertex<T>? finishVertex)
        {
            InitInfo();
            var first = GetVertexInfo(startVertex);
            first.EdgesWeightSum = 0;
            while (true)
            {
                var current = FindUnvisitedVertexWithMinSum();
                if (current == null)
                {
                    break;
                }
                SetSumToNextVertex(current);
            }
            return GetPath(startVertex, finishVertex);
        }

        /// <summary>
        /// Вычисление суммы весов ребер для следующей вершины
        /// </summary>
        /// <param name="info">Информация о текущей вершине</param>
        void SetSumToNextVertex(VertexInfo<T> info)
        {
            info.IsUnvisited = false;
            foreach (var e in info.Vertex.Edges)
            {
                var nextInfo = GetVertexInfo(e.ConnectedVertex);
                var sum = info.EdgesWeightSum + e.EdgeWeight;
                if (sum < nextInfo.EdgesWeightSum)
                {
                    nextInfo.EdgesWeightSum = sum;
                    nextInfo.PreviousVertex = info.Vertex;
                }
            }
        }

        /// <summary>
        /// Формирование пути
        /// </summary>
        /// <param name="startVertex">Начальная вершина</param>
        /// <param name="endVertex">Конечная вершина</param>
        /// <returns>Путь</returns>
        private IEnumerable<Vertex<T>> GetPath(Vertex<T> startVertex, Vertex<T> endVertex)
        {
            List<Vertex<T>> invertPath = new List<Vertex<T>>();
            invertPath.Add(endVertex);
            while (startVertex != endVertex)
            {
                endVertex = GetVertexInfo(endVertex).PreviousVertex;
                invertPath.Add(endVertex);
            }

            invertPath.Reverse();
            return invertPath;
        }
    }
}