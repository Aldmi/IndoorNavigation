﻿namespace ApplicationCore.Shared.DataStruct.GraphNotOriented.DijkstraAlgoritm
{
    /// <summary>
    /// Информация о вершине
    /// </summary>
    public class VertexInfo
    {
        public VertexInfo(Vertex vertex)
        {
            Vertex = vertex;
            IsUnvisited = true;
            EdgesWeightSum = int.MaxValue;
            PreviousVertex = null;
        }
        
        /// <summary>
        /// Вершина
        /// </summary>
        public Vertex Vertex { get; }

        /// <summary>
        /// Не посещенная вершина
        /// </summary>
        public bool IsUnvisited { get; set;}

        /// <summary>
        /// Сумма весов ребер
        /// </summary>
        public int EdgesWeightSum { get; set;}

        /// <summary>
        /// Предыдущая вершина
        /// </summary>
        public Vertex PreviousVertex { get; set;}
    }
}