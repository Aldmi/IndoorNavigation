using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Libs.Beacons.Models;

namespace Test.Beacons.Moq
{
    public static class BeaconFlowData
    {
        /// <summary>
        /// 
        /// </summary>
        public static IObservable<Beacon> CreateFlowImmediatly_ForOneBeacon()
        {
            var listBeacons= new List<Beacon>
            {
                CreateBeaconMajor1(1, -56, -50),
                CreateBeaconMajor1(1, -52, -50),
                CreateBeaconMajor1(1, -54, -50),
                CreateBeaconMajor1(1, -60, -50),
                CreateBeaconMajor1(1, -52, -50),
            };
            var sourse = listBeacons.ToObservable();
            return sourse;
        }
        
        
        // /// <summary>
        // /// 
        // /// </summary>
        // public static IObservable<Beacon> CreateFlow_Fingerprints_7Steps_2metersByOne()
        // {
        //     var txPower = -59;
        //     var sourse=  Observable.Timer(DateTimeOffset.Now.AddSeconds(1.0), TimeSpan.FromSeconds(0.6))
        //         .Take(20)
        //         .Select(x =>
        //         {
        //             var listBeacons= new List<Beacon>
        //             {
        //                 CreateBeaconMajor1(1, -56, txPower),
        //                 CreateBeaconMajor1(2, -52, txPower),
        //                 CreateBeaconMajor1(3, -54, txPower),
        //                 CreateBeaconMajor1(4, -60, txPower),
        //             };
        //
        //             return listBeacons;
        //         })
        //         .SelectMany(list =>list);
        //
        //     return sourse;
        // }



        
        /// <summary>
        /// Движемся по прямой 7 шагов, через 2 метра каждый
        /// </summary>
        public static IObservable<Beacon> CreateFlow_Fingerprints_7Steps_2metersByOne()
        {
            var generateTime = TimeSpan.FromSeconds(0.2);
            async IAsyncEnumerable<Beacon> GenerateDatas()
            {
                var txPower = -59;
                //STEP 1
                yield return CreateBeaconMajor1(1, -56, txPower);//0м
                yield return CreateBeaconMajor1(2, -74, txPower);//5м
                yield return CreateBeaconMajor1(3, -80, txPower);//10м
                yield return CreateBeaconMajor1(4, -86, txPower);//15м
                await Task.Delay(generateTime);
                //STEP 2
                yield return CreateBeaconMajor1(1, -65, txPower);//2м
                yield return CreateBeaconMajor1(2, -69, txPower);//3м
                yield return CreateBeaconMajor1(3, -78, txPower);//8м
                yield return CreateBeaconMajor1(4, -83, txPower);//13м
                await Task.Delay(generateTime);
                //STEP 3
                yield return CreateBeaconMajor1(1, -71, txPower);//4м
                yield return CreateBeaconMajor1(2, -59, txPower);//1м
                yield return CreateBeaconMajor1(3, -75, txPower);//6м
                yield return CreateBeaconMajor1(4, -82, txPower);//11м
                await Task.Delay(generateTime);
                //STEP 4
                yield return CreateBeaconMajor1(1, -75, txPower);//6м
                yield return CreateBeaconMajor1(2, -59, txPower);//1м
                yield return CreateBeaconMajor1(3, -71, txPower);//4м
                yield return CreateBeaconMajor1(4, -80, txPower);//9м
                await Task.Delay(generateTime);
                //STEP 5
                yield return CreateBeaconMajor1(1, -78, txPower);//8м
                yield return CreateBeaconMajor1(2, -69, txPower);//3м
                yield return CreateBeaconMajor1(3, -65, txPower);//2м
                yield return CreateBeaconMajor1(4, -77, txPower);//7м
                await Task.Delay(generateTime);
                //STEP 6
                yield return CreateBeaconMajor1(1, -81, txPower);//10м
                yield return CreateBeaconMajor1(2, -74, txPower);//5м
                 yield return CreateBeaconMajor1(3, -59, txPower);//0м
                 yield return CreateBeaconMajor1(4, -74, txPower);//5м
                await Task.Delay(generateTime);
                //STEP 7
                yield return CreateBeaconMajor1(1, -82, txPower);//12м
                yield return CreateBeaconMajor1(2, -74, txPower);//7м
                yield return CreateBeaconMajor1(3, -65, txPower);//2м
                yield return CreateBeaconMajor1(4, -69, txPower);//3м
                await Task.Delay(generateTime);
            }
            
            return GenerateDatas().ToObservable();
        }
        

        
        public static Beacon CreateBeaconMajor1(ushort minor, int rssi, int txPower)
        {
            return new(new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, minor), rssi, txPower);
        }
    }
}