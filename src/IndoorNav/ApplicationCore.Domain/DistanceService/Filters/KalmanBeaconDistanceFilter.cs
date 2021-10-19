using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ApplicationCore.Domain.DistanceService.Model;
using ApplicationCore.Shared.Services;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.DistanceService.Filters
{
    /// <summary>
    /// Фильтр Калмана для double значения расстояния до Beacon метки. 
    /// </summary>
    public class KalmanBeaconDistanceFilter
    {
        private readonly double _q;
        private readonly double _r;
        private readonly double _covariance;
        private readonly double _f;
        private readonly double _h;
        private readonly TimeSpan? _rottenTime;
        private readonly List<PersonalFilterForEachBeacon> _filtredList= new List<PersonalFilterForEachBeacon>();
        
        
        public KalmanBeaconDistanceFilter(double q, double r, double covariance, double f = 1, double h = 1, TimeSpan? rottenTime = null)
        {
            _q = q;
            _r = r;
            _covariance = covariance;
            _f = f;
            _h = h;
            _rottenTime = rottenTime;
        }
        public KalmanBeaconDistanceFilter(double q, double r, double covariance, TimeSpan rottenTime)
        {
            _q = q;
            _r = r;
            _covariance = covariance;
            _f = 1;
            _h = 1;
            _rottenTime = rottenTime;
        }

        
        /// <summary>
        /// Фильтр с большой ошибкой во входных данных
        /// </summary>
        public static KalmanBeaconDistanceFilter CreateLargeMeasurementErrorFilter(TimeSpan rottenTime) => new KalmanBeaconDistanceFilter(0.8,15,0.1, rottenTime);
        /// <summary>
        /// Фильтр с маленькой ошибкой во входных данных
        /// </summary>
        public static KalmanBeaconDistanceFilter CreateSmallMeasurementErrorFilter(TimeSpan rottenTime) => new KalmanBeaconDistanceFilter(1,2,0.1, rottenTime);

        
        /// <summary>
        /// Выполнить фильтрацию калмана
        /// </summary>
        /// <param name="bd"></param>
        /// <returns></returns>
        public BeaconDistance Filtrate(BeaconDistance bd)
        {
            var filter = _filtredList.FirstOrDefault(f => f.BeaconId == bd.BeaconId);
            KalmanFilterSimple1D kalman;
            if (filter == null)
            {
                kalman = new KalmanFilterSimple1D(_q, _r, _f, _h);
                kalman.SetState(bd.Distance, _covariance);
                kalman.Correct(bd.Distance);
                _filtredList.Add(new PersonalFilterForEachBeacon(bd, kalman));
            }
            else
            {
                kalman = filter.Kalman;
                //Если фильтр протух, то обновим состояние фильтра
                if (_rottenTime.HasValue && filter.IsRotten(_rottenTime.Value))
                {
                    kalman.SetState(bd.Distance, _covariance);
                }
                filter.RefreshLastSeen(bd.LastSeen); //обновим метку синхронизации.
                kalman.Correct(bd.Distance);         //выполним предсказания
            }
            var filtredDistance = kalman.State;
            Debug.WriteLine($"{bd.Distance:F1} -> {filtredDistance:F1}");
            return bd.CreateWithNewDistance(filtredDistance);
        }
        
        
        private class PersonalFilterForEachBeacon 
        {
            public KalmanFilterSimple1D Kalman { get; }
            public BeaconId BeaconId { get; }
            public DateTimeOffset LastSeen { get; private set; }

            
            public PersonalFilterForEachBeacon(BeaconDistance beaconDistance, KalmanFilterSimple1D kalman)
            {
                BeaconId = beaconDistance.BeaconId;
                LastSeen = beaconDistance.LastSeen;
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
}