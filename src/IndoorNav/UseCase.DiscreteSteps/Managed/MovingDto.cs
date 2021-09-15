using System;
using ApplicationCore.Domain.CheckPointModel;
using ApplicationCore.Domain.MovingService.Model;

namespace UseCase.DiscreteSteps.Managed
{
    public class MovingDto
    {
        public MovingDto(CheckPointBase? start, CheckPointBase? end, MovingEvent movingEvent)
        {
            Start = start;
            End = end;
            MovingEvent = movingEvent;
            LastSeen = DateTimeOffset.UtcNow;
        }
        
        public CheckPointBase? Start { get; }
        public CheckPointBase? End { get; }
        public MovingEvent MovingEvent { get; }
        public DateTimeOffset LastSeen { get; }

        
        public override string ToString()
        {
            var res= MovingEvent switch
            {
                MovingEvent.Unknown => $"Unknown",
                MovingEvent.InitSegment => $"Init: '{Start.Description.Name}'",
                MovingEvent.StartSegment => $"Start: '{Start.Description.Name}'",
                MovingEvent.GoTo => $"GoToEnd: '{Start.Description.Name}'- ...",
                MovingEvent.CompleteSegment => $"Complete: '{Start.Description.Name} - {End.Description.Name}'",
                _ => throw new ArgumentOutOfRangeException()
            };
            return $"{res}   [{LastSeen:HH:mm:ss:ffff}]";
        }
    }
}