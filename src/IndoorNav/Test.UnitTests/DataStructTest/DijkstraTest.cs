using System.Diagnostics;
using ApplicationCore.Shared.DataStruct.GraphNotOriented;
using ApplicationCore.Shared.DataStruct.GraphNotOriented.DijkstraAlgoritm;
using Xunit;

namespace Test.Beacons.DataStructTest
{
    public class DijkstraTest
    {
        public Graph CreateGraph()
        {
            var g = new Graph();
            
            // //добавление вершин
            // g.AddVertex("A");
            // g.AddVertex("B");
            // g.AddVertex("C");
            // g.AddVertex("D");
            // g.AddVertex("E");
            // g.AddVertex("F");
            // g.AddVertex("G");
            //
            // //добавление ребер
            // g.AddEdge("A", "B", 22);
            // g.AddEdge("A", "C", 33);
            // g.AddEdge("A", "D", 61);
            // g.AddEdge("B", "C", 47);
            // g.AddEdge("B", "E", 93);
            // g.AddEdge("C", "D", 11);
            // g.AddEdge("C", "E", 79);
            // g.AddEdge("C", "F", 63);
            // g.AddEdge("D", "F", 41);
            // g.AddEdge("E", "F", 17);
            // g.AddEdge("E", "G", 58);
            // g.AddEdge("F", "G", 84);
            // return g;
            
            
            //добавление вершин
            g.AddVertex("A");
            g.AddVertex("B");
            g.AddVertex("C");

            
            //добавление ребер
            g.AddEdge("A", "C", 22);
            g.AddEdge("B", "C", 33);
            return g;
        }
        
        
        [Fact]
        public void GetMatrixTest()
        {
            //arrange
            var g = CreateGraph();
            
            //act
            var matrixWeight = g.GetWeightMatrix();

            //assert
            for (int i = 0; i < g.VerticesCount; i++)
            {
                for (int j = 0; j < g.VerticesCount; j++)
                {
                   Debug.Write(matrixWeight[i,j].ToString("D2") + " ");
                }
                Debug.WriteLine("");
            }
        }
        
        
        [Fact]
        public void FindShortestPathTest()
        {
            //arrange
            var g = CreateGraph();
            var dijkstra = new Dijkstra(g);
            
            //act
            var path = dijkstra.FindShortestPath("A", "G");
            
            //assert
        }
    }
}