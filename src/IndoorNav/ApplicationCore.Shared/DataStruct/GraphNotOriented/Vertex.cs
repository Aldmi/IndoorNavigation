using System.Collections.Generic;

namespace ApplicationCore.Shared.DataStruct.GraphNotOriented
{
    /// <summary>
    /// Вершина графа
    /// </summary>
    public class Vertex
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="vertexName">Название вершины</param>
        public Vertex(string vertexName)
        {
            Name = vertexName;
            Edges = new List<Edge>();
        }
        
        /// <summary>
        /// Название вершины
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Список ребер
        /// </summary>
        public List<Edge> Edges { get; }

        
        /// <summary>
        /// Добавить ребро
        /// </summary>
        /// <param name="newEdge">Ребро</param>
        public void AddEdge(Edge newEdge)
        {
            Edges.Add(newEdge);
        }

        /// <summary>
        /// Добавить ребро от текущей врешины с другой
        /// </summary>
        /// <param name="vertex">Вершина</param>
        /// <param name="edgeWeight">Вес</param>
        public void AddEdge(Vertex vertex, int edgeWeight)
        {
            AddEdge(new Edge(vertex, edgeWeight));
        }

        /// <summary>
        /// Преобразование в строку
        /// </summary>
        /// <returns>Имя вершины</returns>
        public override string ToString() => Name;
    }
}