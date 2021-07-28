using System;
using System.Reactive.Linq;
using Libs.Beacons.Models;
using Libs.BluetoothLE;

namespace Libs.Beacons.Platforms
{
    public static class BleManagerExtensions
    {
        public static IObservable<Beacon> ScanForBeacons(this IBleManager manager, BleScanType scanType) =>
            manager
                .Scan(new ScanConfig
                {
                    //AndroidUseScanBatching = true,
                    ScanType = scanType
                })
                .Where(x => x.IsBeacon())
                .Select(x => x.AdvertisementData.ManufacturerData.Data.Parse(x.Rssi));


        public static bool IsBeacon(this ScanResult result)
        {
            var md = result.AdvertisementData?.ManufacturerData;

            if (md == null || md.Data == null || md.Data.Length != 23)
                return false;

            if (md.CompanyId != 76)
                return false;

            return md.Data.IsBeaconPacket();
        }
    }
}