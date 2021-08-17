using System;
using ApplicationCore.Domain.DiscreteSteps;

namespace UseCase.DiscreteSteps.Managed
{
    public class MovingDto
    {
        public MovingDto(CheckPoint? start, CheckPoint? end, MovingEvent movingEvent)
        {
            Start = start;
            End = end;
            MovingEvent = movingEvent;
            LastSeen = DateTimeOffset.UtcNow;
        }
        
        public CheckPoint? Start { get; }
        public CheckPoint? End { get; }
        public MovingEvent MovingEvent { get; }
        public DateTimeOffset LastSeen { get; }

        
        public override string ToString()
        {
            var res= MovingEvent switch
            {
                MovingEvent.Unknown => $"Unknown",
                MovingEvent.InitSegment => $"Init: '{Start.Description.Name}'",
                MovingEvent.StartSegment => $"Start: '{Start.Description.Name}'",
                MovingEvent.GoToEnd => $"GoToEnd: '{Start.Description.Name}'- ...",
                MovingEvent.CompleteSegment => $"Complete: '{Start.Description.Name} - {End.Description.Name}'",
                _ => throw new ArgumentOutOfRangeException()
            };
            return $"{res}   [{LastSeen:T}]";
        }
    }
}