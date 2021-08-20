using System;
using System.Collections.Generic;

namespace ApplicationCore.Shared.DataStruct.GraphNotOriented
{
    /// <summary>
    /// Вершина графа
    /// </summary>
    public class Vertex<T> : IEquatable<Vertex<T>> where T : IEquatable<T>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="vertexValue">Данные</param>
        public Vertex(T vertexValue)
        {
            Value = vertexValue;
            Edges = new List<Edge<T>>();
        }
        
        /// <summary>
        /// Данные в вершине
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Список ребер
        /// </summary>
        public List<Edge<T>> Edges { get; }

        
        /// <summary>
        /// Добавить ребро
        /// </summary>
        /// <param name="newEdge">Ребро</param>
        public void AddEdge(Edge<T> newEdge)
        {
            Edges.Add(newEdge);
        }

        /// <summary>
        /// Добавить ребро от текущей врешины с другой
        /// </summary>
        /// <param name="vertex">Вершина</param>
        /// <param name="edgeWeight">Вес</param>
        public void AddEdge(Vertex<T> vertex, int edgeWeight)
        {
            AddEdge(new Edge<T>(vertex, edgeWeight));
        }

        /// <summary>
        /// Преобразование в строку
        /// </summary>
        /// <returns>Имя вершины</returns>
        public override string ToString() => Value.ToString();

        
        #region Equatable
        public static bool operator ==(Vertex<T> left, Vertex<T>? right) => Equals(left, right);
        public static bool operator !=(Vertex<T>? left, Vertex<T>? right) => !Equals(left, right);
        
        public bool Equals(Vertex<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value.Equals(other.Value) && Edges.Equals(other.Edges);
        }
        
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Vertex<T>) obj);
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(Value, Edges);
        }
        #endregion
    }
}