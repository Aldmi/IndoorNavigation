using System;
using ApplicationCore.Domain.CheckPointModel;

namespace ApplicationCore.Domain.MovingService.Model
{
    /// <summary>
    /// Объект перемещения их CheckPoint start в CheckPoint end.
    /// </summary>
    public class Moving
    {
        private Moving(CheckPointBase? start, CheckPointBase? end, MovingEvent movingEvent)
        {
            Start = start;
            End = end;
            MovingEvent = movingEvent;
            LastSeen= DateTimeOffset.UtcNow;
        }

        public CheckPointBase? Start { get; }
        public CheckPointBase? End { get; }
        public MovingEvent MovingEvent { get; }
        public DateTimeOffset LastSeen { get; }



        public static Moving UnknownSegment() => new Moving(null, null, MovingEvent.Unknown);
        public static Moving InitSegment(CheckPointBase start) => new Moving(start, null, MovingEvent.InitSegment);
        public static Moving StartSegment(CheckPointBase start) => new Moving(start, null, MovingEvent.StartSegment);
        public static Moving GoToEnd(CheckPointBase start) => new Moving(start, null, MovingEvent.GoTo);
        public static Moving CompleteSegment(CheckPointBase start, CheckPointBase end) => new Moving(start, end, MovingEvent.CompleteSegment);


        public override string ToString()
        {
            return MovingEvent switch
            {
                MovingEvent.Unknown => $"Unknown",
                MovingEvent.InitSegment => $"InitSegment: '{Start.Description.Name}'",
                MovingEvent.StartSegment => $"StartSegment: '{Start.Description.Name}'",
                MovingEvent.GoTo => $"GoTo: '{Start.Description.Name}'- ...",
                MovingEvent.CompleteSegment => $"CompleteSegment: '{Start.Description.Name} - {End.Description.Name}'",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    public enum MovingEvent
    {
        Unknown,
        InitSegment,
        StartSegment,
        GoTo,
        CompleteSegment 
    }
}