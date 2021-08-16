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
        }
        
        public CheckPoint? Start { get; }
        public CheckPoint? End { get; }
        public MovingEvent MovingEvent { get; }
    }
}