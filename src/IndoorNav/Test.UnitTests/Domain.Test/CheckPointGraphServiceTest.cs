using System;
using System.Collections.Generic;
using ApplicationCore.Domain.DiscreteSteps;
using ApplicationCore.Shared.DataStruct.GraphNotOriented;
using Libs.Beacons.Models;
using Test.Beacons.Domain.Test.Data;
using Xunit;

namespace Test.Beacons.Domain.Test
{
    public class CheckPointGraphServiceTest
    {
        [Fact]
        public void CalculateMoveTest()
        {
            //arrange
            var graph = CheckPointGraphSampleOne.CreateSimpleGraph();
            var service = new CheckPointGraphService(graph);
            var route = CheckPointGraphSampleOne.Route0To6();

            //act
           var res0= service.CalculateMove(route[0]);
           var res1= service.CalculateMove(route[1]);
           var res2= service.CalculateMove(route[2]);
           var res3= service.CalculateMove(route[3]);
           var res4= service.CalculateMove(route[4]);
           var res5= service.CalculateMove(route[5]);
           var res6= service.CalculateMove(route[6]);
           var res7= service.CalculateMove(route[7]);
           var res8= service.CalculateMove(route[8]);
           
           //assert
        }
    }
}