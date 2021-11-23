using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using ApplicationCore.Domain.RssiFingerprinting.Statistic;
using ApplicationCore.Shared.Filters.Kalman;
using ApplicationCore.Shared.Helpers;
using Libs.Beacons.Flows;
using Libs.Beacons.Models;

namespace UseCase.RssiFingerprinting.Flows
{
    /// <summary>
    /// Собирает статистику по работе фильтра калмана 1D
    /// </summary>
    public static class Kalman1DFilterStatisticFlow
    {
        public static IObservable<IList<AfterKalman1DStatistic>> BeaconKalman1DStatistic(this IObservable<Beacon> sourse,
            TimeSpan bufferTime,
            Kalman1DFilterWrapper kalman1D,
            double maxDistance)
        {
            var flowBeaconAverages = sourse
                //Буфферизация и разбиение на группы по BeaconId
                .GroupAfterBuffer(bufferTime)
                //Среднее из сигналов Rssi для каждого датчика
                .CalcAverageRssiInGroupBeacons(RssiHelpers.CalculateAverageRssi)
                //Убрать маленькие AverageRssi (пересчитанные к Distance)
               // .RemoveSmallBeaconAverage(maxDistance)
                //Статистика после фильтра Калмана 1D
                .StatisticFiltredByKalman1D(kalman1D);
            
            return flowBeaconAverages;
        }
        
        private static IObservable<IList<AfterKalman1DStatistic>> StatisticFiltredByKalman1D(this IObservable<IList<BeaconAverage>> sourse,
            Kalman1DFilterWrapper kalman1D)
        {
            return sourse.Select(list => list.Select(ba =>
                {
                    var rssiBefore = ba.Rssi;
                    var rssiAfter = kalman1D.Filtrate(ba.BeaconId, ba.Rssi);
                    var distanceBefore=RssiHelpers.CalculateDistance(ba.TxPower, rssiBefore).Value;
                    var distanceAfetr=RssiHelpers.CalculateDistance(ba.TxPower, rssiAfter).Value;
                    return new AfterKalman1DStatistic(ba.BeaconId,rssiBefore, rssiAfter, distanceBefore, distanceAfetr, ba.LastSeen);
                })
                .ToList());
        }
    }
}