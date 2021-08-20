using System;

namespace ApplicationCore.Shared.DataStruct.GraphNotOriented
{
    /// <summary>
    /// Ребро графа
    /// </summary>
    public class Edge<T> : IEquatable<Edge<T>> where T : IEquatable<T>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="connectedVertex">Связанная вершина</param>
        /// <param name="weight">Вес ребра</param>
        public Edge(Vertex<T> connectedVertex, int weight)
        {
            ConnectedVertex = connectedVertex;
            EdgeWeight = weight;
        }
        
        /// <summary>
        /// Связанная вершина
        /// </summary>
        public Vertex<T> ConnectedVertex { get; }

        /// <summary>
        /// Вес ребра
        /// </summary>
        public int EdgeWeight { get; }
        

        #region Equatable
        public static bool operator ==(Edge<T> left, Edge<T> right) => Equals(left, right);
        public static bool operator !=(Edge<T> left, Edge<T> right) => !Equals(left, right);
        
        public bool Equals(Edge<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ConnectedVertex.Equals(other.ConnectedVertex) && EdgeWeight == other.EdgeWeight;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Edge<T>) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ConnectedVertex, EdgeWeight);
        }
        #endregion
    }
}