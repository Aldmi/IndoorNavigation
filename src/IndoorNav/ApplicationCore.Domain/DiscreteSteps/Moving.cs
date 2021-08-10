namespace ApplicationCore.Domain.DiscreteSteps
{
    public class Moving
    {
        private Moving(CheckPoint? start, CheckPoint? end)
        {
            Start = start;
            End = end;

            if (Start != null && End == null)
            {
                GetMovingEvent = MovingEvent.StartSegment;
            }
            else if (Start != null && End != null)
            {
                GetMovingEvent = MovingEvent.CompleteSegment;
            }
            else
            {
                GetMovingEvent= MovingEvent.Unknown;
            }
        }

        public CheckPoint? Start { get; }
        public CheckPoint? End { get; }
        public MovingEvent GetMovingEvent { get; }



        public static Moving UnknownSegment() => new Moving(null, null);
        public static Moving StartSegment(CheckPoint start) => new Moving(start, null);
        public static Moving CompleteSegment(CheckPoint start, CheckPoint end) => new Moving(start, end);
    }

    public enum MovingEvent
    {
        Unknown,
        StartSegment,
        CompleteSegment 
    }
}