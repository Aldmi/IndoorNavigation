using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Libs.Beacons.Models;

namespace ApplicationCore.Shared.Filters.Kalman
{
    /// <summary>
    /// Обертка над однолмерным фильбтром Калмана.
    /// Для каждого фильтруемого значения (кажжого BeaconId) будет создаваться свой фильтр, с  флагом "протухания" значения.
    /// Если данные долго не посупают на фильтр из спсика, то такой фильтр протухший, и в нем сбрасывается значениена новое.
    /// Фильтр Калмана для double значения.
    /// </summary>
    public class Kalman1DFilterWrapper
    {
        private readonly double _q;
        private readonly double _r;
        private readonly double _covariance;
        private readonly double _f;
        private readonly double _h;
        private readonly TimeSpan? _rottenTime;
        private readonly List<PersonalKalman1DForEachBeacon> _personalFilterList= new();
        
        
        public Kalman1DFilterWrapper(double q, double r, double covariance, double f = 1, double h = 1, TimeSpan? rottenTime = null)
        {
            _q = q;
            _r = r;
            _covariance = covariance;
            _f = f;
            _h = h;
            _rottenTime = rottenTime;
        }
        public Kalman1DFilterWrapper(double q, double r, double covariance, TimeSpan rottenTime)
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
        public static Kalman1DFilterWrapper GetLargeMeasurementErrorFilterWrapper => new Kalman1DFilterWrapper(0.8,15,0.1, TimeSpan.FromSeconds(5));
        /// <summary>
        /// Фильтр с маленькой ошибкой во входных данных
        /// </summary>
        public static Kalman1DFilterWrapper GetSmallMeasurementErrorFilterWrapper => new Kalman1DFilterWrapper(1,2,0.1, TimeSpan.FromSeconds(5));

        
        /// <summary>
        /// Выполнить фильтрацию калмана
        /// </summary>
        public double Filtrate(BeaconId beaconId, double state)
        {
            var filter = _personalFilterList.FirstOrDefault(f => f.BeaconId == beaconId);
            var lastSeen = DateTimeOffset.UtcNow;
            Kalman1DFilterSimple kalman1D;
            if (filter == null)
            {
                kalman1D = new Kalman1DFilterSimple(_q, _r, _f, _h);
                kalman1D.SetState(state, _covariance);
                kalman1D.Correct(state);
                _personalFilterList.Add(new PersonalKalman1DForEachBeacon(beaconId, lastSeen, kalman1D));
            }
            else
            {
                kalman1D = filter.Kalman1D;
                //Если фильтр протух, то обновим состояние фильтра
                if (_rottenTime.HasValue && filter.IsRotten(_rottenTime.Value))
                {
                    kalman1D.SetState(state, _covariance);
                }
                filter.RefreshLastSeen(lastSeen);   //обновим метку синхронизации.
                kalman1D.Correct(state);             //выполним предсказания
            }
            var filtredState = kalman1D.State;
            Debug.WriteLine($"{state:F1} -> {filtredState:F1}");
            return filtredState;
        }
    }
    
    internal class PersonalKalman1DForEachBeacon 
    {
        public Kalman1DFilterSimple Kalman1D { get; }
        public BeaconId BeaconId { get; }
        public DateTimeOffset LastSeen { get; private set; }

            
        public PersonalKalman1DForEachBeacon(BeaconId beaconId, DateTimeOffset lastSeen, Kalman1DFilterSimple kalman1D)
        {
            BeaconId = beaconId;
            LastSeen = lastSeen;
            Kalman1D = kalman1D;
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