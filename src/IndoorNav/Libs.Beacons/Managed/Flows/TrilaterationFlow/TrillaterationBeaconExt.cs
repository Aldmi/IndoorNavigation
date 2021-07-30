using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Libs.Beacons.Managed.Options;
using Libs.Beacons.Models;

namespace Libs.Beacons.Managed.Flows.TrilaterationFlow
{
    public static class TrillaterationBeaconExt
    {
        public static IObservable<Beacon> WhenWhiteList(this IObservable<Beacon> sourse, IEnumerable<BeaconOption> options)
        {
            return sourse.Where(beacon => options.FirstOrDefault(b => b.Equal2Beacon(beacon)) != null);
        }
        
        
        public static IObservable<IList<Sphere>> CreateSphere(this IObservable<IList<Beacon>> sourse, IEnumerable<BeaconOption> options)
        {
            return sourse.Select(listBeacons => listBeacons
                .Select(b=>
                {
                    var option = options.FirstOrDefault(o => o.Equal2Beacon(b));
                    var center = option?.Ancore ?? Point.EmptyPoint;
                    var n= option?.N ?? 1;
                    var txPower = option?.TxPower ?? b.TxPower;
                    var radius = Algoritm.CalculateDistance(txPower, b.Rssi);
                    return new Sphere(b, center, radius );
                })
                .ToList());
        }
    }


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
    }
}
