using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// Вычислить среднее значение rssi из группы сигналов.
        /// </summary>
        /// <param name="sourse"></param>
        /// <param name="averageRssiCalc"></param>
        /// <returns></returns>
        public static IObservable<IList<BeaconAverage>> CalcAverageRssiInGroupBeacons(this IObservable<List<IGrouping<BeaconId, Beacon>>> sourse, Func<List<int>, double> averageRssiCalc)
        {
            return sourse
                .Select(listGr =>
                {
                    var inDataList = listGr.Select(group =>
                    {
                        var id = group.Key;
                        var txPower = group.First().TxPower;
                        var rssiList = group
                            .Select(b => b.Rssi)
                            .ToList();
                        
                        var averageRssi= averageRssiCalc(rssiList);
                        var model = new BeaconAverage(id, averageRssi, txPower);
                        return model;
                    }).ToList();
                    
                    Debug.WriteLine(inDataList.Count);
                    
                    return inDataList;
                });
        }
        
        
        /// <summary>
        /// Фильтрация маяков по белому списку BeaconId.
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