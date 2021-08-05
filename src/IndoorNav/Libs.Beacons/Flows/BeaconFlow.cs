using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Libs.Beacons.Models;

namespace Libs.Beacons.Flows
{
    public static class BeaconFlow
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
                    .Where(list=>list.Any())
                    .Select(beacons => beacons.GroupBy(b => b.Id).ToList());
        }
        
        
        /// <summary>
        /// Фильтрация маяков по белому списку Id.
        /// </summary>
        /// <param name="sourse"></param>
        /// <param name="whiteList"></param>
        /// <returns></returns>
        public static IObservable<Beacon> WhenWhiteList(this IObservable<Beacon> sourse, IEnumerable<BeaconId> whiteList)
        {
            return sourse.Where(beacon => whiteList.FirstOrDefault(id => id == beacon.Id) != null);
        }
    }
}