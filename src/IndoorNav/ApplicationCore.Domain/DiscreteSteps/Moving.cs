using System;

namespace ApplicationCore.Domain.DiscreteSteps
{
    public class Moving
    {
        private Moving(CheckPoint? start, CheckPoint? end, MovingEvent movingEvent)
        {
            Start = start;
            End = end;
            MovingEvent = movingEvent;
        }

        public CheckPoint? Start { get; }
        public CheckPoint? End { get; }
        public MovingEvent MovingEvent { get; }



        public static Moving UnknownSegment() => new Moving(null, null, MovingEvent.Unknown);
        public static Moving InitSegment(CheckPoint start) => new Moving(start, null, MovingEvent.InitSegment);
        public static Moving StartSegment(CheckPoint start) => new Moving(start, null, MovingEvent.StartSegment);
        public static Moving GoToEnd(CheckPoint start) => new Moving(start, null, MovingEvent.GoToEnd);
        public static Moving CompleteSegment(CheckPoint start, CheckPoint end) => new Moving(start, end, MovingEvent.CompleteSegment);


        public override string ToString()
        {
            return MovingEvent switch
            {
                MovingEvent.Unknown => $"Unknown",
                MovingEvent.InitSegment => $"InitSegment: '{Start.Description.Name}'",
                MovingEvent.StartSegment => $"StartSegment: '{Start.Description.Name}'",
                MovingEvent.GoToEnd => $"GoToEnd: '{Start.Description.Name}'- ...",
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
        GoToEnd,
        CompleteSegment 
    }
}