using System;
using Libs.Beacons.Models;

namespace ApplicationCore.Shared.Services
{
    public class PersonalFilterForEachBeacon 
    {
        public KalmanFilterSimple1D Kalman { get; }
        public BeaconId BeaconId { get; }
        public DateTimeOffset LastSeen { get; private set; }

            
        public PersonalFilterForEachBeacon(BeaconId beaconId, DateTimeOffset lastSeen, KalmanFilterSimple1D kalman)
        {
            BeaconId = beaconId;
            LastSeen = lastSeen;
            Kalman = kalman;
        }

        /// <summary>
        /// Протух.
        /// LastSeen устарелло
        /// </summary>
        public bool IsRotten(TimeSpan validOffset)
        {
            var offset = DateTimeOffset.UtcNow - LastSeen;
            return offset > validOffset;
        }

        /// <summary>
        /// ОЛбновить дату синхронизации.
        /// </summary>
        public void RefreshLastSeen(DateTimeOffset lastSeen )
        {
            LastSeen = lastSeen;
        }
    }
}