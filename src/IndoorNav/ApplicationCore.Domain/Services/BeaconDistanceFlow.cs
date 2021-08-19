using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using ApplicationCore.Domain.DiscreteSteps;
using ApplicationCore.Shared;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.Services
{
    public static class BeaconDistanceFlow
    {
        public static IObservable<IList<BeaconDistanceModel>> Map2BeaconDistanceModel(
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
                        var rangeList = group
                            .Select(b => Algoritms.CalculateDistance(txPower, b.Rssi))
                            .ToList();

                        var range = distanceHandler(id, rangeList);
                        var model = new BeaconDistanceModel(id, range);
                        return model;
                    }).ToList();
                    return inDataList;
                });
        }
    }
}