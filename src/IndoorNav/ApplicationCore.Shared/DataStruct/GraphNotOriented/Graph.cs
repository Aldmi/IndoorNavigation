using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ApplicationCore.Shared.DataStruct.GraphNotOriented
{
    /// <summary>
    /// Граф
    /// </summary>
    public class Graph
    {
        /// <summary>
        /// Список вершин графа
        /// </summary>
        public List<Vertex> Vertices { get; }
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
            Vertices = new List<Vertex>();
        }

        /// <summary>
        /// Добавление вершины
        /// </summary>
        /// <param name="vertexName">Имя вершины</param>
        public void AddVertex(string vertexName)
        {
            Vertices.Add(new Vertex(vertexName));
        }

        /// <summary>
        /// Поиск вершины
        /// </summary>
        /// <param name="vertexName">Название вершины</param>
        /// <returns>Найденная вершина</returns>
        public Vertex FindVertex(string vertexName)
        {
            foreach (var v in Vertices)
            {
                if (v.Name.Equals(vertexName))
                {
                    return v;
                }
            }
            return null;
        }

        /// <summary>
        /// Добавление ребра
        /// </summary>
        /// <param name="firstName">Имя первой вершины</param>
        /// <param name="secondName">Имя второй вершины</param>
        /// <param name="weight">Вес ребра соединяющего вершины</param>
        public void AddEdge(string firstName, string secondName, int weight)
        {
            var v1 = FindVertex(firstName);
            var v2 = FindVertex(secondName);
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