using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Shared.DataStruct;
using FluentAssertions;
using Xunit;

namespace Test.Beacons.DataStructTest
{
    public class TreeNodeTest
    {
        private class InData
        {
            public InData(int id, string name)
            {
                Id = id;
                Name = name;
            }
            public int Id { get;}
            public string Name { get;}
        }
        
        
                
        #region TheoryData
        public static IEnumerable<object[]> Datas => new[]
        {
            new object[]
            {
              "K1",
              10
            },
            new object[]
            {
                "Kassy",
                101
            },
            new object[]
            {
                "0",
                1
            },
            new object[]
            {
                "ToExit",
                12
            },
            new object[]
            {
                "Not found",
                null
            },
            

        };
        #endregion
        [Theory]
        [MemberData(nameof(Datas))]
        public void FindInDepthTest(string findName, int? expectedId)
        {
            //Arrange
            var root = new TreeNode<InData>(new InData(1,"0"));
            var k1= root.AddChild(new InData(10,"K1"));
            k1.AddChildren(new InData(11,"To Train"), new InData(12,"ToExit"));
            var k2=k1.AddChild(new InData(100,"K2"));
            k2.AddChild(new InData(101,"Kassy"));
            
                    /*          
                     k2 - - -Kassy
                     |
                     |
    To Train - - -  k1 - - - ToExit
                     |
                     |
                    root             
                    */
                    
           //act
           //var n= root.FindInDepth(node => node.Value.Name == "Kassy");
           var node= root.FindInDepth(node => node.Value.Name == findName);
           var val = node?.Value.Id;
           val.Should().Be(expectedId);
        }
        
        
        [Fact]
        public void FlattenTest()
        {
            //Arrange
            var root = new TreeNode<InData>(new InData(1,"0"));
            var k1= root.AddChild(new InData(10,"K1"));
            k1.AddChildren(new InData(11,"To Train"), new InData(12,"ToExit"));
            var k2=k1.AddChild(new InData(100,"K2"));
            k2.AddChild(new InData(101,"Kassy"));
            
            /*          
             k2 - - -Kassy
             |
             |
To Train - - -  k1 - - - ToExit
             |
             |
            root             
            */
                    
            //act
            var flatNodes = root.Flatten().ToList();
            flatNodes.Count.Should().Be(6);
        }
    }
}