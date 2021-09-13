using System;
using ApplicationCore.Domain.CheckPointModel;
using ApplicationCore.Domain.MovingService.Model;

namespace ApplicationCore.Domain.MovingService
{
    public class MovingCsvStatistic
    {
        private const string Separator = ";";
        public static readonly string CsvHeader = $"{nameof(MovingEvent)}{Separator}{nameof(Start)}{Separator}{nameof(End)}{Separator}{nameof(LastSeen)}";

        
        private MovingCsvStatistic(CheckPointBase? start, CheckPointBase? end, MovingEvent movingEvent, DateTimeOffset lastSeen)
        {
            Start = start;
            End = end;
            MovingEvent = movingEvent;
            LastSeen = lastSeen;
        }


        public CheckPointBase? Start { get; }
        public CheckPointBase? End { get; }
        public MovingEvent MovingEvent { get; }
        public DateTimeOffset LastSeen { get; }
        
        
        public static MovingCsvStatistic Create(Moving moving)
        {
            return new MovingCsvStatistic(moving.Start, moving.End, moving.MovingEvent, moving.LastSeen);
        }
        
        
        public string Convert2CsvFormat()
        {
            return $"{MovingEvent}{Separator}{Start}{Separator}{End}{Separator}{LastSeen:hh:mm:ss}";
        }
    }
}