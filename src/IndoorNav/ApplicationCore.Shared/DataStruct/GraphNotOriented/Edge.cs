namespace ApplicationCore.Shared.DataStruct.GraphNotOriented
{
    /// <summary>
    /// Ребро графа
    /// </summary>
    public class Edge
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="connectedVertex">Связанная вершина</param>
        /// <param name="weight">Вес ребра</param>
        public Edge(Vertex connectedVertex, int weight)
        {
            ConnectedVertex = connectedVertex;
            EdgeWeight = weight;
        }
        
        /// <summary>
        /// Связанная вершина
        /// </summary>
        public Vertex ConnectedVertex { get; }

        /// <summary>
        /// Вес ребра
        /// </summary>
        public int EdgeWeight { get; }
    }
}