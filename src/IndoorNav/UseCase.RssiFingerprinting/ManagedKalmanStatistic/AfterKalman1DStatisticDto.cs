using System;
using ApplicationCore.Domain.RssiFingerprinting.Statistic;
using Shiny;

namespace UseCase.RssiFingerprinting.ManagedKalmanStatistic
{
    public class AfterKalman1DStatisticDto : NotifyPropertyChanged
    {
        public AfterKalman1DStatisticDto(AfterKalman1DStatistic afterKalman1DStatistic) => AfterKalman1DStatistic = afterKalman1DStatistic;
        
        public string StrId => $"{AfterKalman1DStatistic.BeaconId.Uuid}\nMajor= {AfterKalman1DStatistic.BeaconId.Major}  Minor= {AfterKalman1DStatistic.BeaconId.Minor}";
        
        public AfterKalman1DStatistic AfterKalman1DStatistic { get; }

        private double _rssiBefore;
        public double RssiBefore
        {
            get => _rssiBefore;
            internal set => Set(ref _rssiBefore, value);
        }
        
        
        private double _rssiAfter;
        public double RssiAfter
        {
            get => _rssiAfter;
            internal set => Set(ref _rssiAfter, value);
        }
        
        
        private double _distanceBefore;
        public double DistanceBefore
        {
            get => _distanceBefore;
            internal set => Set(ref _distanceBefore, value);
        }
        
        
        private double _distanceAfter;
        public double DistanceAfter
        {
            get => _distanceAfter;
            internal set => Set(ref _distanceAfter, value);
        }
        
        
        private DateTimeOffset _lastSeen;
        public DateTimeOffset LastSeen
        {
            get => _lastSeen;
            internal set => Set(ref _lastSeen, value);
        }
    }
}