using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Libs.Beacons.Models;

namespace Libs.Beacons.Managed.TrilaterationFlow
{
    public static class TrillaterationBeaconExt
    {
        public static IObservable<IList<Sphere>> CreateEmptySphere(this IObservable<IList<Beacon>> sourse)
        {
            return sourse.Select(listBeacons => listBeacons.Select(b=> new Sphere(b)).ToList());
        }
        
        
        public static IObservable<IList<Sphere>> AddRadius(this IObservable<IList<Sphere>> sourse, uint n)
        {
            return sourse.Do(listSphere =>
            {
                foreach (var sphere in listSphere)
                {
                    var distance = Algoritm.CalculateDistance(-77, sphere.Beacon.Rssi);
                    sphere.SetRadius(distance);
                }
            } );
        }
        
        
        public static IObservable<IList<Sphere>> AddCenter(this IObservable<IList<Sphere>> sourse, Point center)
        {
            return sourse.Do(listSphere =>
            {
                foreach (var sphere in listSphere)
                {
                    sphere.SetCenter(center);
                }
            });
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
