using System;
using System.Collections.Generic;
using ApplicationCore.Domain.DiscreteSteps;
using ApplicationCore.Domain.DiscreteSteps.Model;
using ApplicationCore.Domain.Navigation;
using ApplicationCore.Domain.Navigation.Model;
using ApplicationCore.Shared.DataStruct.GraphNotOriented;
using FluentAssertions;
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
            var service = new GraphMovingCalculator(graph);
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
           res0.MovingEvent.Should().Be(MovingEvent.InitSegment);
           res0.Start.Description.Name.Should().Be("Коридор 1");
           res0.End.Should().BeNull();
           
           res1.MovingEvent.Should().Be(MovingEvent.StartSegment);
           res1.Start.Description.Name.Should().Be("Коридор 1");
           res1.End.Should().BeNull();
           
           res2.MovingEvent.Should().Be(MovingEvent.GoTo);
           res2.Start.Description.Name.Should().Be("Коридор 1");
           res2.End.Should().BeNull();
           
           res3.MovingEvent.Should().Be(MovingEvent.GoTo);
           res3.Start.Description.Name.Should().Be("Коридор 1");
           res3.End.Should().BeNull();
           
           res4.MovingEvent.Should().Be(MovingEvent.CompleteSegment);
           res4.Start.Description.Name.Should().Be("Коридор 1");
           res4.End.Description.Name.Should().Be("Коридор 2");
           
           res5.MovingEvent.Should().Be(MovingEvent.StartSegment);
           res5.Start.Description.Name.Should().Be("Коридор 2");
           res5.End.Should().BeNull();
           
           res6.MovingEvent.Should().Be(MovingEvent.GoTo);
           res6.Start.Description.Name.Should().Be("Коридор 2");
           res6.End.Should().BeNull();
           
           res7.MovingEvent.Should().Be(MovingEvent.CompleteSegment);
           res7.Start.Description.Name.Should().Be("Коридор 2");
           res7.End.Description.Name.Should().Be("Кассы");
           
           res8.MovingEvent.Should().Be(MovingEvent.StartSegment);
           res8.Start.Description.Name.Should().Be("Кассы");
           res8.End.Should().BeNull();
        }
    }
}