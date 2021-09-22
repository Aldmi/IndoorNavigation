using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using ApplicationCore.Domain.DistanceService.Model;
using ApplicationCore.Domain.MovingService.Model;
using ApplicationCore.Shared;
using CSharpFunctionalExtensions;
using Libs.Beacons.Flows;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.DistanceService
{
    public static class BeaconDistanceFlow
    {
        public static IObservable<IList<BeaconDistance>>Beacon2BeaconDistance(this IObservable<Beacon> sourse,
            TimeSpan bufferTime,
            double beaconHeight,
            int txPower,
            Func<List<int>, int> averageRssiCalc)
        {
            return sourse
                //Буфферизация и разбиение на группы по Id
                .GroupAfterBuffer(bufferTime)
                //Среднее из сигналов Rssi одного датчика
                .CalcAverageRssiInGroupBeacons(averageRssiCalc)
                //Rssi -> Distance
                .MapBeaconRssi2BeaconDistanceResult(beaconHeight, txPower)
                //Вернуть только валидные Distance
                .OnlyValidDistance()
                //Упорядочить по Distance
                .OrderByDescendingForDistance();
        }
        
        
        /// <summary>
        /// Сырые Beacon данные преобразует к расстоянию от маяка и обрабатывает массив расстояний с помощью distanceHandler.
        /// </summary>
        /// <param name="sourse"></param>
        /// <param name="bufferTime"></param>
        /// <param name="txPower"></param>
        /// <param name="distanceHandler"></param>
        /// <returns></returns>
        public static IObservable<IList<BeaconDistance>>Beacon2BeaconDistance(this IObservable<Beacon> sourse,
            TimeSpan bufferTime,
            int txPower,
            Func<BeaconId, IEnumerable<double>, double> distanceHandler)
        {
            return sourse
                //Буфферизация и разбиение на группы по Id
                .GroupAfterBuffer(bufferTime)
                //Маппинг к InData. (фильтрация значений в группе и преобразование к InData)
                .Map2BeaconDistance(distanceHandler, txPower)
                //Упорядочить по Distance
                .OrderByDescendingForDistance();
        }
        
        
        /// <summary>
        /// Сырые Beacon данные преобразует к расстоянию от маяка и обрабатывает массив расстояний с помощью distanceHandler.
        /// </summary>
        /// <param name="sourse"></param>
        /// <param name="bufferTime"></param>
        /// <param name="txPower"></param>
        /// <param name="distanceHandler"></param>
        /// <returns></returns>
        public static IObservable<IList<BeaconDistanceStatistic>>Beacon2BeaconDistanceStatistic(this IObservable<Beacon> sourse,
            TimeSpan bufferTime,
            int txPower,
            Func<BeaconId, IEnumerable<double>, double> distanceHandler)
        {
            return sourse
                .GroupAfterBuffer(bufferTime)
                .Map2BeaconDistanceStatistic(distanceHandler, txPower);
        }


        /// <summary>
        /// Обработка сигналов Rssi, у маяков
        /// Для каждой группы сигналов 
        /// 1. Вычислим расстяние по прямой (гипотенуза) до маяка на потолке
        /// 2. Вычислим проекцию на ось X, зная расстояние до маяка.
        /// 
        /// </summary>
        /// <param name="sourse">список групп Beacon, сгрупированных по BeaconId</param>
        /// <param name="beaconHeight">Высота маяка над приемником</param>
        /// <param name="txPower"></param>
        public static IObservable<IList<Result<BeaconDistance>>> MapBeaconRssi2BeaconDistanceResult(
            this IObservable<IList<Beacon>> sourse,
            double beaconHeight,
            int txPower)
        {
            return sourse
                .Select(listBeacons =>
                {
                    var inDataList = listBeacons.Select(beacon =>
                    {
                        var beaconDistanceResult = Rssi2DistanceConverter.CalculateDistance2(txPower, beacon.Rssi)
                            .Bind(hypotenuseDistance => DistanceXProjectionCalculator.CalculateXProjection(hypotenuseDistance, beaconHeight))
                            .Map(xProjection => new BeaconDistance(beacon.Id, xProjection));
                           
                       return beaconDistanceResult; 
                    }).ToList();
                    return inDataList;
                });
        }
        
        
        /// <summary>
        /// Обработка сигналов, накопленных за определенное время и сгрупированных по BeaconId.
        /// </summary>
        /// <param name="sourse">список групп Beacon, сгрупированных по BeaconId</param>
        /// <param name="distanceHandler">Rssi преобразуется к distance</param>
        /// <param name="txPower"></param>
        public static IObservable<IList<BeaconDistance>> Map2BeaconDistance(
            this IObservable<List<IGrouping<BeaconId, Beacon>>> sourse,
            Func<BeaconId, IEnumerable<double>, double> distanceHandler,
            int txPower)
        {
            return sourse
                .Select(listGr =>
                {
                    var inDataList = listGr.Select(group =>
                    {
                        var id = group.Key;
                        var distanceList = group
                            .Select(b => Rssi2DistanceConverter.CalculateDistance(txPower, b.Rssi))
                            .ToList();
                        
                        var distance = distanceHandler(id, distanceList);
                        var model = new BeaconDistance(id, distance);
                        return model;
                    }).ToList();
                    return inDataList;
                });
        }
        
        
        /// <summary>
        /// Обработка сигналов, накопленных за определенное время и сгрупированных по BeaconId.
        /// В виде объекта статистики, показывающего список входных сигналов rssi их преобразование к списку distance
        /// </summary>
        /// <param name="sourse">список групп Beacon, сгрупированных по BeaconId</param>
        /// <param name="distanceHandler">Rssi преобразуется к distance</param>
        /// <param name="txPower"></param>
        public static IObservable<IList<BeaconDistanceStatistic>> Map2BeaconDistanceStatistic(
            this IObservable<List<IGrouping<BeaconId, Beacon>>> sourse,
            Func<BeaconId, IEnumerable<double>, double> distanceHandler,
            int txPower)
        {
            return sourse
                .Select(listGr =>
                {
                    var inDataList = listGr.Select(group =>
                    {
                        var id = group.Key;
                        var distanceList = group
                            .Select(b => Rssi2DistanceConverter.CalculateDistance(txPower, b.Rssi))
                            .ToList();
                        
                        var distance = distanceHandler(id, distanceList);
                        var model = new BeaconDistanceStatistic(id, distanceList, distance);
                        return model;
                    }).ToList();
                    return inDataList;
                });
        }
        
        
        /// <summary>
        /// Упорядочить список InputData по Distance (в порядке убывания).
        /// </summary>
        public static IObservable<IList<BeaconDistance>> OrderByDescendingForDistance(this IObservable<IList<BeaconDistance>> sourse)
        {
            return sourse
                .Select(list =>
                {
                    return list.OrderByDescending(inData => inData.Distance).ToList();
                });
        }
        
        
        /// <summary>
        /// Упорядочить список InputData по Distance (в порядке убывания).
        /// </summary>
        /// OrderByDescendingForDistance
        public static IObservable<IList<BeaconDistance>> OnlyValidDistance(this IObservable<IList<Result<BeaconDistance>>> sourse)
        {
            return sourse
                .Select(list =>
                {
                    return list.Where(distanceRes => distanceRes.IsSuccess)
                        .Select(distanceRes=> distanceRes.Value).ToList().
                        ToList();
                });
        }
        
        

    }
}