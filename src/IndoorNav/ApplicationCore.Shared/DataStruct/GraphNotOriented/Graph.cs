using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace ApplicationCore.Shared.DataStruct.GraphNotOriented
{
    /// <summary>
    /// Граф
    /// </summary>
    public class Graph<T> where T : IEquatable<T>
    {
        /// <summary>
        /// Список вершин графа
        /// </summary>
        public List<Vertex<T>> Vertices { get; }
        public int VerticesCount => Vertices.Count;
        public int EdgesCount()
        {
            //перечислять все Vertices и добавлять в список все Edges каждой вершины.
            //Удалить повторяющиеся Edge.
            return 10;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public Graph()
        {
            Vertices = new List<Vertex<T>>();
        }

        /// <summary>
        /// Добавление вершины
        /// </summary>
        /// <param name="vertexValue">Данные</param>
        public void AddVertex(T vertexValue)
        {
            Vertices.Add(new Vertex<T>(vertexValue));
        }

        
        /// <summary>
        /// Поиск вершины по значению.
        /// </summary>
        /// <returns>Найденная вершина</returns>
        public Vertex<T>? FindVertex(T value)
        {
            return FindVertex(v=> v.Value.Equals(value));
        }
        
        
        /// <summary>
        /// Поиск вершины.
        /// </summary>
        /// <param name="predicate">условия поиска вершины</param>
        /// <returns>Найденная вершина</returns>
        public Vertex<T>? FindVertex(Func<Vertex<T>, bool> predicate)
        {
            return Vertices.FirstOrDefault(predicate);
        }
        
        
        /// <summary>
        /// Добавление ребра
        /// </summary>
        /// <param name="firstValue">Имя первой вершины</param>
        /// <param name="secondValue">Имя второй вершины</param>
        /// <param name="weight">Вес ребра соединяющего вершины</param>
        public void AddEdge(T firstValue, T secondValue, int weight)
        {
            var v1 = FindVertex(vertex=>vertex.Value.Equals(firstValue));
            var v2 = FindVertex(vertex=>vertex.Value.Equals(secondValue));
            if (v2 != null && v1 != null)
            {
                v1.AddEdge(v2, weight);
                v2.AddEdge(v1, weight);
            }
        }

        /// <summary>
        /// Вернуть матрицу весов
        /// </summary>
        /// <returns></returns>
        public int[,] GetWeightMatrix()
        {
            var matrix = new int[Vertices.Count, Vertices.Count];
            for (var i = 0; i < Vertices.Count; i++)
            {
                var vertex = Vertices[i];
                var column = i;
                foreach (var edge in vertex.Edges)
                {
                    var row = Vertices.IndexOf(edge.ConnectedVertex);
                    matrix[column, row] = edge.EdgeWeight;
                }
            }
            return matrix;
        }
    }
}