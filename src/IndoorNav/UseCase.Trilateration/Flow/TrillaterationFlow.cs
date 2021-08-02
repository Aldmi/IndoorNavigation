using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Libs.Beacons.Flows;
using Libs.Beacons.Models;
using Microsoft.Extensions.Logging;
using UseCase.Trilateration.Model;
using UseCase.Trilateration.Services;

namespace UseCase.Trilateration.Flow
{
    public static class TrillaterationFlow
    {
        /// <summary>
        /// Конструктор потока для сервиса ManagedScan.
        /// </summary>
        /// <param name="sourse"></param>
        /// <param name="whiteList"></param>
        /// <param name="bufferTime"></param>
        /// <param name="sphereFactory"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static IObservable<IList<Sphere>> ManagedScanFlow(this IObservable<Beacon> sourse,
            IEnumerable<BeaconId> whiteList,
            TimeSpan bufferTime,
            SphereFactory sphereFactory,
            ILogger? logger = null)
        {
            return sourse
                //Проходят только значения из списка
                .WhenWhiteList(whiteList)
                //Буфферизация и разбиение на группы по Id
                .GroupAfterBuffer(bufferTime)
                //Создание сфер
                .CreateSphere(sphereFactory);
        }
        
        
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


    //TODO: вынести в Domain
    public static class Algoritm
    {
        public static double CalculateDistance(int measuredPower, double rssi) 
        {
            if (rssi == 0) {
                return -1.0; // if we cannot determine distance, return -1.
            }
            double ratio = rssi*1.0/measuredPower;
            if (ratio < 1.0) {
                return Math.Pow(ratio,10);
            }
            else {
                double distance =  (0.89976)*Math.Pow(ratio,7.7095) + 0.111;
                return distance;
            }
        }
        
        
        public static double CalculateDistance(int measuredPower, int rssi) 
        {
            if (rssi == 0) {
                return -1.0; // if we cannot determine distance, return -1.
            }
            double ratio = rssi*1.0/measuredPower;
            if (ratio < 1.0) {
                return Math.Pow(ratio,10);
            }
            else {
                double distance =  (0.89976)*Math.Pow(ratio,7.7095) + 0.111;
                return distance;
            }
        }
    }
}
