using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Libs.Beacons.Managed.Flows.TrilaterationFlow;
using Libs.Beacons.Models;
using Debug = System.Diagnostics.Debug;

namespace Libs.Beacons.Managed.Flows.FilterFlow
{
    public static class BeaconExt
    {
        /// <summary>
        /// Фильтр среднего значения </summary>
        /// <param name="sourse"></param>
        /// <param name="buferTime"></param>
        /// <param name="algoritm"></param>
        /// <returns></returns>
        public static IObservable<List<IGrouping<BeaconId, Beacon>>> GroupAfterBuffer(this IObservable<Beacon> sourse, TimeSpan buferTime)
        {
            return sourse
                    .Buffer(buferTime)
                    .Select(beacons => beacons.GroupBy(b => b.Id).ToList());
        }
        
        
        
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