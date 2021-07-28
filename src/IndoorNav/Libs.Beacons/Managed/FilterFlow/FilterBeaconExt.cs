using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Android.OS;
using Libs.Beacons.Managed.TrilaterationFlow;
using Libs.Beacons.Models;
using Debug = System.Diagnostics.Debug;

namespace Libs.Beacons.Managed.FilterFlow
{
    public static class FilterBeaconExt
    {
        /// <summary>
        /// Фильтр среднего значения </summary>
        /// <param name="sourse"></param>
        /// <param name="buferTime"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IObservable<IList<Beacon>> AverageFilter(this IObservable<Beacon> sourse, TimeSpan buferTime, Func<IEnumerable<int>, int> filter)
        {
            var filtredBeacons= sourse
                .Buffer(buferTime)
                .Select(beacons =>
                    beacons.GroupBy(b => b.GetHashCode())
                        .Select(group =>
                        {
                            var beaconsInGroup = group.ToList();
#if DEBUG
                            var analiticData = beaconsInGroup.Select(b => $"{b.Major}/{b.Minor}").Aggregate((s1, s2) => s1 + "  "+ s2);
                            Debug.WriteLine($"Count= '{beaconsInGroup.Count}'   {analiticData}");
#endif
                            var averageRssi = filter(beaconsInGroup.Select(b=>b.Rssi));
                            var filtredBeacon= beaconsInGroup.First().CreateByBlank(averageRssi);
                            return filtredBeacon;
                        })
                        .ToList());

            return filtredBeacons;
        }
        
        
        
        public static IObservable<IList<Beacon>> AverageFilterDebug(this IObservable<Beacon> sourse, TimeSpan buferTime, Func<IEnumerable<int>, int> filter)
        {
            var filtredBeacons = sourse
                .Buffer(buferTime);
            
            return filtredBeacons;
        }
    }
}