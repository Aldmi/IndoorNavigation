using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.Trilateration.Spheres
{
    public static class SphereFlow
    {
        /// <summary>
        /// Создание сфер из спсика групп маяков объединенных по BeaconId
        /// </summary>
        /// <param name="sourse"></param>
        /// <param name="sphereFactory"></param>
        /// <returns></returns>
        public static IObservable<IList<Sphere>> CreateSphere(this IObservable<List<IGrouping<BeaconId, Beacon>>> sourse, SphereFactory sphereFactory)
        {
            return sourse
                .Select(listGr =>
                {
                    var sphereList = listGr.Select(group =>
                    {
                        var id = group.Key;
                        var beacons = group.ToList();
                        return sphereFactory.Create(id, beacons);
                    }).ToList();
                    return sphereList;
                });
        }
    }
}