using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.Services
{
    /// <summary>
    /// Обработчик ппорции Distance, учитывая текущую скорость объекта и предыдущее зангчение Distance.
    /// </summary>
    public class BeaconDistanceGivenSpeedHandler : IBeaconDistanceHandler
    {
        private readonly Dictionary<BeaconId, (double dist, DateTimeOffset lastSeen)> _lastDistance = new Dictionary<BeaconId, (double dist, DateTimeOffset lastSeen)>();


        public BeaconDistanceGivenSpeedHandler()
        {
            
        }
        
        
        public double Invoke(BeaconId id, IEnumerable<double> distances)
        {
            //1. Если набор значений новый, обработаем его и добавим в словарь.
            if (!_lastDistance.ContainsKey(id))
            {
              return AddNewDistance(id, distances);
            }
            //2. Если значение в словаре устарело, добавим его как новое.
            var (expectedMin, expectedMax) = GetExpectedDistanceRange(id);
            if ((expectedMin, expectedMax) == (0, 0))
            {
                return AddNewDistance(id, distances);
            }
            double dist;
            //3. Если есть валидные значения в группе, то вычислим среднее из них.
            var validList = distances.Where(d =>d >= expectedMin && d <= expectedMax);
            if (validList.Any())
            { 
                dist = validList.Average();
            }
            //4. Если ВСЕ значения в группе НЕ валидны, то определим наиболее подходящее.
            else
            {
                dist = Guess((expectedMin, expectedMax), distances);
            }
            _lastDistance[id] = (dist, DateTimeOffset.UtcNow);
            return dist;
        }

        
        private double AddNewDistance(BeaconId id, IEnumerable<double> distances)
        {
            var newDist = distances.Average();
            _lastDistance.Add(id, (newDist, DateTimeOffset.UtcNow));
            return newDist;
        }

        
        /// <summary>
        /// Получить ожидаемый диапазон перемещения
        /// </summary>
        private (double expectedMin, double expectedMax) GetExpectedDistanceRange(BeaconId id)
        {
            var (dist, lastSeen) = _lastDistance[id];
            var maxAge = DateTimeOffset.UtcNow.Subtract(lastSeen);
            if (maxAge > TimeSpan.FromSeconds(1))
            {
                return (0, 0);
            }
            var speed = GetSpeed();
            var expectedMin = dist - speed;
            var expectedMax = dist + speed;
            return (expectedMin, expectedMax);
        }
        
        
        private double Guess((double expectedMin, double expectedMax) expectedDistanceRange, IEnumerable<double> distances)
        {

            
            return 10;
        }
        
        /// <summary>
        /// получить скорость из стороннего сервиса
        /// </summary>
        public double GetSpeed()
        {
            return 1.5;
        }
    }
}