using System;
using ApplicationCore.Domain.CheckPointModel.Trilateration.Spheres;
using ApplicationCore.Domain.DistanceService.Model;
using Libs.Beacons;
using Shiny;

namespace UseCase.Trilateration.Managed
{
    public class BeaconDistanceStatisticDto : NotifyPropertyChanged
    {
        public BeaconDistanceStatisticDto(BeaconDistanceStatistic statistic) => Statistic = statistic;
        public BeaconDistanceStatistic Statistic { get; }
        
        
        public string StrId => $"{Statistic.BeaconId.Uuid}\nMajor= {Statistic.BeaconId.Major}  Minor= {Statistic.BeaconId.Minor}";  
        
        
        private string _distanceList;
        public string DistanceList
        {
            get => _distanceList;
            internal set => Set(ref _distanceList, value);
        }
        
        private string _distanceResult;
        public string DistanceResult
        {
            get => _distanceResult;
            internal set => Set(ref _distanceResult, value);
        }
        
        private DateTimeOffset _lastSeen;
        public DateTimeOffset LastSeen
        {
            get => _lastSeen;
            internal set => Set(ref _lastSeen, value);
        }
    }
}