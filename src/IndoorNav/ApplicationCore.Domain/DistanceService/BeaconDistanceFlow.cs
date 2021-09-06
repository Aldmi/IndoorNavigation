using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using ApplicationCore.Domain.DistanceService.Model;
using ApplicationCore.Shared;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.DistanceService
{
    public static class BeaconDistanceFlow
    {
        /// <summary>
        /// Обработка сигналов, накопленных за определенное время и сгрупированных по BeaconId.
        /// </summary>
        /// <param name="sourse">список групп Beacon, сгрупированных по BeaconId</param>
        /// <param name="distanceHandler">Rssi преобразуется к distance</param>
        /// <param name="txPower"></param>
        public static IObservable<IList<BeaconDistance>> Map2BeaconDistanceModel(
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
                            .Select(b => Algoritms.CalculateDistance(txPower, b.Rssi))
                            .ToList();

                        var distance = distanceHandler(id, distanceList);
                        var model = new BeaconDistance(id, distance);
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
    }
}