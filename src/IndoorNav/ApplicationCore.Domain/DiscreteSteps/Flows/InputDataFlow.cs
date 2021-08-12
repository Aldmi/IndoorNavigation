using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using ApplicationCore.Domain.Trilateration.Spheres;
using ApplicationCore.Shared;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.DiscreteSteps.Flows
{
    public static class InputDataFlow
    {
        /// <summary>
        /// Преобразовать группы Beacon к InputData.
        /// </summary>
        public static IObservable<IList<InputData>> Map2InData(this IObservable<List<IGrouping<BeaconId, Beacon>>> sourse, int txPower)
        {
            return sourse
                .Select(listGr =>
                {
                    var inDataList = listGr.Select(group =>
                    {
                        var id = group.Key;
                        var averageDistance = group.Average(b => Algoritms.CalculateDistance(txPower, b.Rssi));
                        var inData = new InputData(id, averageDistance);
                        return inData;
                    }).ToList();
                    return inDataList;
                });
        }
        
        /// <summary>
        /// Упорядочить список InputData по Range.
        /// </summary>
        public static IObservable<IList<InputData>> OrderByDescendingForRange(this IObservable<IList<InputData>> sourse)
        {
            return sourse
                .Select(list =>
                {
                    return list.OrderByDescending(inData => inData.Range).ToList();
                });
        }
    }
}