using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.DistanceService.Services
{
    public class DistanceHandler
    {
        private readonly IEnumerable<IDistanceFilter> _filters;
        private readonly IDistanceAggregator _aggregator;
        private readonly Dictionary<BeaconId, PreviousDistance> _previousDistanceDict= new Dictionary<BeaconId, PreviousDistance>();


        public DistanceHandler(IEnumerable<IDistanceFilter> filters, IDistanceAggregator aggregator)
        {
            _filters = filters;
            _aggregator = aggregator;
        }

        
        public double Aggregate(BeaconId id, IEnumerable<double> distances)
        {
            var previousDistance = GetPreviousDistance(id);
            IEnumerable<double> filtredDistances = _filters.Aggregate(distances, (currentDistanceList, filter) => filter.Invoke(previousDistance, currentDistanceList));
            var aggregateDistance = _aggregator.Invoke(filtredDistances);
            SetPreviousDistance(id, aggregateDistance);
            return aggregateDistance;
        }


        /// <summary>
        /// Получить предыдущее значение.
        /// Проверить устарело ли значение по TimeStamp.
        /// </summary>
        private double? GetPreviousDistance(BeaconId id)
        {
            if (_previousDistanceDict.TryGetValue(id, out var previousDistance))
            {
                //Проверить TimeStamp у объекта distance если он меньше заданной дельты, то веренм distance, если нет то вернем null. (После всех обработчиков значение перепишется новым вычисленным с новым TimeStamp)
                return previousDistance.Distance;
            }
            return null;
        }
        
        
        /// <summary>
        /// Установить значение.
        /// </summary>
        private void SetPreviousDistance(BeaconId id, double distance)
        {
            _previousDistanceDict[id] = new PreviousDistance(id, distance, DateTimeOffset.Now);
        }
        
        
        /// <summary>
        /// Предыдущая вычисленная дистанция.
        /// </summary>
        private class PreviousDistance
        {
            public PreviousDistance(BeaconId id, double distance, DateTimeOffset lastSeen)
            {
                Id = id;
                Distance = distance;
                LastSeen = lastSeen;
            }

            public BeaconId Id { get; }
            public double Distance { get; }
            public DateTimeOffset LastSeen{ get; }
        }
    }
    
}