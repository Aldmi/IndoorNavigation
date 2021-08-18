using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using ApplicationCore.Domain.DiscreteSteps;
using ApplicationCore.Shared;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.Services
{
    public static class BeaconDistanceModelFlow
    {
        public static IObservable<IList<BeaconDistanceModel>> Map2BeaconDistanceModel(
            this IObservable<List<IGrouping<BeaconId, Beacon>>> sourse,
            DistanceHandlerService handler)
        {
            return sourse
                .Select(listGr =>
                {
                    var inDataList = listGr.Select(group =>
                    {
                        var id = group.Key;
                        var rangeList = group
                            .Select(b => Algoritms.CalculateDistance(-77, b.Rssi))
                            .ToList();

                        var range = handler.Invoke(id, rangeList);
                        var model = new BeaconDistanceModel(id, range);
                        return model;
                    }).ToList();
                    return inDataList;
                });
        }
    }
}