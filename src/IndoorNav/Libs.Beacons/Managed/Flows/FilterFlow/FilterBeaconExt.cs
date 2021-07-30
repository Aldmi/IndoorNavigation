using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Libs.Beacons.Models;
using Debug = System.Diagnostics.Debug;

namespace Libs.Beacons.Managed.Flows.FilterFlow
{
    public static class FilterBeaconExt
    {
        /// <summary>
        /// Фильтр среднего значения </summary>
        /// <param name="sourse"></param>
        /// <param name="buferTime"></param>
        /// <param name="algoritm"></param>
        /// <returns></returns>
        public static IObservable<IList<Beacon>> AverageFilter(this IObservable<Beacon> sourse, TimeSpan buferTime, Func<IEnumerable<int>, int> algoritm)
        {
            var filtredBeacons= sourse
                .Buffer(buferTime)
                .Select(beacons =>
                    beacons.GroupBy(b => b.GetHashCode())
                        .Select(group =>
                        {
                            var beaconsInGroup = group.ToList();
                            var averageRssi = algoritm(beaconsInGroup.Select(b=>b.Rssi));
#if DEBUG
                           // var analiticData = beaconsInGroup.Select(b => $"{b.Major}/{b.Minor}={b.Rssi}").Aggregate((s1, s2) => s1 + "  "+ s2);
                            var analiticData = $"'{beaconsInGroup.First().Major}/{beaconsInGroup.First().Minor}'   ({beaconsInGroup.Select(b => $"{b.Rssi}").Aggregate((s1, s2) => s1 + "+"+ s2)}) / {beaconsInGroup.Count} = {averageRssi}";
                            Debug.WriteLine($"{DateTimeOffset.UtcNow:HH:mm:ss}  Count='{beaconsInGroup.Count}'   '{analiticData}'");
#endif
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