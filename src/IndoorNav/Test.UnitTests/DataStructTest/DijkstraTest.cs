using System;
using System.Diagnostics;
using ApplicationCore.Shared.DataStruct.GraphNotOriented;
using ApplicationCore.Shared.DataStruct.GraphNotOriented.DijkstraAlgoritm;
using Xunit;

namespace Test.Beacons.DataStructTest
{
    public class DijkstraTest
    {
        public class InData : IEquatable<InData>
        {
            public InData(int id, string name)
            {
                Id = id;
                Name = name;
            }
            public int Id { get;}
            public string Name { get;}

            

            public bool Equals(InData other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Id == other.Id && Name == other.Name;
            }
            
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((InData) obj);
            }
            
            public override int GetHashCode()
            {
                return HashCode.Combine(Id, Name);
            }
        }
        
        
        public Graph<InData> CreateGraph()
        {
            var g = new Graph<InData>();
            
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
            var dataA = new InData(1, "A");
            var dataB = new InData(2, "B");
            var dataC = new InData(3, "C");
            var dataD = new InData(4, "D");
            
            g.AddVertex(dataA);
            g.AddVertex(dataB);
            g.AddVertex(dataC);
            g.AddVertex(dataD);

            
            //добавление ребер
            g.AddEdge(dataA, dataC, 22);
            g.AddEdge( dataB , dataC, 33);
            g.AddEdge( dataC , dataD, 10);
            g.AddEdge( dataB , dataD, 15);
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
            var dijkstra = new Dijkstra<InData>(g);
            
            //act
            var path = dijkstra.FindShortestPath(new InData(1, "A"), new InData(4, "D"));
            
            //assert
        }
    }
}