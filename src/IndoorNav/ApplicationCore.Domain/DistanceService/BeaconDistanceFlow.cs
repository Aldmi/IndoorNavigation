using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using ApplicationCore.Domain.DistanceService.Filters;
using ApplicationCore.Domain.DistanceService.Model;
using ApplicationCore.Shared.Helpers;
using CSharpFunctionalExtensions;
using Libs.Beacons.Flows;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.DistanceService
{
    public static class BeaconDistanceFlow
    {
        public static IObservable<IList<BeaconDistance>> Beacon2BeaconDistance(this IObservable<Beacon> sourse,
            TimeSpan bufferTime,
            double beaconHeight,
            KalmanBeaconDistanceFilter? kalmanDistanceFilter,
            double maxDistance)
        {
            var flow = sourse
                //Буфферизация и разбиение на группы по Id
                .GroupAfterBuffer(bufferTime)
                //Среднее из сигналов Rssi для каждого датчика
                .CalcAverageRssiInGroupBeacons(RssiHelpers.CalculateAverageRssi)
                //AverageRssi -> Distance
                .MapBeaconRssi2BeaconDistanceResult(beaconHeight)
                //Вернуть только валидные Distance
                .OnlyValidDistance()
                //Пропускать тоолко значения меньше maxDistance
                .WhereMaxDistance(maxDistance);
            
            if (kalmanDistanceFilter != null)
            {
                flow = flow
                    //Отфильтровать экстремумы (фильтр кламана 1D)
                    .FiltredByKalman1D(kalmanDistanceFilter);
            }
            
            flow= flow
                //Упорядочить по Distance
                .OrderByDescendingForDistance();
            
            return flow;
        }
        
        /// <summary>
        /// Фильтр убирает экстремумы.
        /// </summary>
        private static IObservable<IList<BeaconDistance>> FiltredByKalman1D(this IObservable<IList<BeaconDistance>> sourse,
            KalmanBeaconDistanceFilter kalmanDistanceFilter)
        {
            return sourse
                .Select(list => list
                    .Select(beaconDistance => kalmanDistanceFilter.Filtrate(beaconDistance))
                    .ToList());
        }
        
        
        /// <summary>
        /// Обработка сигналов Rssi, у маяков
        /// Для каждой группы сигналов 
        /// 1. Вычислим расстяние по прямой (гипотенуза) до маяка на потолке
        /// 2. Вычислим проекцию на ось X, зная расстояние до маяка.
        /// </summary>
        /// <param name="sourse">список групп Beacon, сгрупированных по BeaconId</param>
        /// <param name="beaconHeight">Высота маяка над приемником</param>
        private static IObservable<IList<Result<BeaconDistance>>> MapBeaconRssi2BeaconDistanceResult(
            this IObservable<IList<BeaconAverage>> sourse,
            double beaconHeight)
        {
            return sourse
                .Select(listBeacons =>
                {
                    var inDataList = listBeacons.Select(beacon =>
                    {
                        var beaconDistanceResult = RssiHelpers.CalculateDistance(beacon.TxPower, beacon.Rssi)
                            .Bind(hypotenuseDistance =>
                                MathHelpers.CalculateXProjection(hypotenuseDistance, beaconHeight))
                            .Map(xProjection => new BeaconDistance(beacon.Id, xProjection));

                        return beaconDistanceResult;
                    }).ToList();
                    return inDataList;
                });
        }
        
        
        /// <summary>
        /// Вернуть только BeaconDistance с Result == IsSuccess
        /// </summary>
        private static IObservable<IList<BeaconDistance>> OnlyValidDistance(this IObservable<IList<Result<BeaconDistance>>> sourse)
        {
            return sourse.Select(list =>
            {
                return list
                    .Where(distanceRes => distanceRes.IsSuccess)
                    .Select(distanceRes => distanceRes.Value)
                    .ToList();
            });
        }
        
        
        /// <summary>
        /// where фильтр, проходят только Distance <= maxDist
        /// </summary>
        private static IObservable<IList<BeaconDistance>> WhereMaxDistance(this IObservable<IList<BeaconDistance>>  sourse, double maxDistance)
        {
            return sourse.Select(list =>
            {
                return list
                    .Where(bd => bd.Distance <= maxDistance)
                    .ToList();
            });
            
        }
        
        /// <summary>
        /// Упорядочить список InputData по Distance (в порядке убывания).
        /// </summary>
        private static IObservable<IList<BeaconDistance>> OrderByDescendingForDistance(this IObservable<IList<BeaconDistance>> sourse)
        {
            return sourse.Select(list => { return list.OrderByDescending(inData => inData.Distance).ToList(); });
        }
    }
}