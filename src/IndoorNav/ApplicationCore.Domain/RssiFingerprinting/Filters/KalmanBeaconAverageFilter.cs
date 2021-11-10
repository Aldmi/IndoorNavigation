using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ApplicationCore.Domain.DistanceService.Model;
using ApplicationCore.Shared.Services;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.RssiFingerprinting.Filters
{
    /// <summary>
    /// Фильтр Калмана для double значения расстояния до Beacon метки. 
    /// </summary>
    public class KalmanBeaconAverageFilter
    {
        private readonly double _q;
        private readonly double _r;
        private readonly double _covariance;
        private readonly double _f;
        private readonly double _h;
        private readonly TimeSpan? _rottenTime;
        private readonly List<PersonalFilterForEachBeacon> _personalFilterList= new();
        
        
        public KalmanBeaconAverageFilter(double q, double r, double covariance, double f = 1, double h = 1, TimeSpan? rottenTime = null)
        {
            _q = q;
            _r = r;
            _covariance = covariance;
            _f = f;
            _h = h;
            _rottenTime = rottenTime;
        }
        public KalmanBeaconAverageFilter(double q, double r, double covariance, TimeSpan rottenTime)
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
        public static KalmanBeaconAverageFilter GetLargeMeasurementErrorFilter => new KalmanBeaconAverageFilter(0.8,15,0.1, TimeSpan.FromSeconds(5));
        /// <summary>
        /// Фильтр с маленькой ошибкой во входных данных
        /// </summary>
        public static KalmanBeaconAverageFilter GetSmallMeasurementErrorFilter => new KalmanBeaconAverageFilter(1,2,0.1, TimeSpan.FromSeconds(5));

        
        /// <summary>
        /// Выполнить фильтрацию калмана
        /// </summary>
        /// <param name="ba"></param>
        /// <returns></returns>
        public BeaconAverage Filtrate(BeaconAverage ba)
        {
            var filter = _personalFilterList.FirstOrDefault(f => f.BeaconId == ba.BeaconId);
            KalmanFilterSimple1D kalman;
            if (filter == null)
            {
                kalman = new KalmanFilterSimple1D(_q, _r, _f, _h);
                kalman.SetState(ba.Rssi, _covariance);
                kalman.Correct(ba.Rssi);
                _personalFilterList.Add(new PersonalFilterForEachBeacon(ba.BeaconId, ba.LastSeen, kalman));
            }
            else
            {
                kalman = filter.Kalman;
                //Если фильтр протух, то обновим состояние фильтра
                if (_rottenTime.HasValue && filter.IsRotten(_rottenTime.Value))
                {
                    kalman.SetState(ba.Rssi, _covariance);
                }
                filter.RefreshLastSeen(ba.LastSeen); //обновим метку синхронизации.
                kalman.Correct(ba.Rssi);             //выполним предсказания
            }
            var filtredRssi = kalman.State;
            Debug.WriteLine($"{ba.Rssi:F1} -> {filtredRssi:F1}");
            return ba.CreateWithNewRssi(filtredRssi);
        }
        
    }
}