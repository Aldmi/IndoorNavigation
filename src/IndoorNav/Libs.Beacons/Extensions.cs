using System;
using System.Diagnostics;
using System.Linq;
using Libs.Beacons.Models;
using Microsoft.Extensions.Logging;

namespace Libs.Beacons
{
    public static class Extensions
    {
        // public static ManagedScan CreateManagedScan(this IBeaconRangingManager manager, ILogger<ManagedScan> logger)
        //     => new ManagedScan(manager, logger);


        public static bool IsBeaconInRegion(this BeaconRegion region, Beacon beacon)
        {
            if (!region.Uuid.Equals(beacon.Uuid))
                return false;

            if (region.Major == null && region.Minor == null)
                return true;

            if (region.Major != beacon.Major)
                return false;

            if (region.Minor == null || region.Minor == beacon.Minor)
                return true;

            return false;
        }


        public static Beacon CreateByBlank(this Beacon blank, int newRssi)
        { 
           var accuracy = CalculateAccuracy(blank.TxPower, newRssi);
           var proximity = CalculateProximity(blank.TxPower, newRssi);
           var newBeacon = new Beacon(blank.Uuid, blank.Major, blank.Minor, proximity, newRssi, accuracy, blank.TxPower);
           return newBeacon;
        }
        
        
        /// <summary>
        /// This should not be called by iOS as txpower is not available in the advertisement packet
        /// </summary>
        /// <returns>The beacon.</returns>
        /// <param name="data">Data.</param>
        /// <param name="rssi">Analitic.</param>
        public static Beacon Parse(this byte[] data, int rssi)
        {
            if (BitConverter.IsLittleEndian)
            {
                var uuidString = BitConverter.ToString(data, 2, 16).Replace("-", String.Empty);
                var uuid = new Guid(uuidString);
                var major = BitConverter.ToUInt16(data.Skip(18).Take(2).Reverse().ToArray(), 0);
                var minor = BitConverter.ToUInt16(data.Skip(20).Take(2).Reverse().ToArray(), 0);
                var txpower = data[22];
                var accuracy = CalculateAccuracy(txpower, rssi);
                var proximity = CalculateProximity(txpower, rssi);
                //Debug.WriteLine(txpower);//DEBUG
                return new Beacon(uuid, major, minor, proximity, rssi, accuracy, txpower);
            }
            throw new ArgumentException("TODO");
        }


        // 6 meters+ = far
        // 2 meters+ = near
        // 0.5 meters += immediate

        public static Proximity CalculateProximity(int txpower, double rssi)
        {
            var distance = Math.Pow(10d, (txpower * -1 - rssi) / 20);
            if (distance >= 6E-6d)
                return Proximity.Far;

            if (distance > 0.5E-6d)
                return Proximity.Near;

            return Proximity.Immediate;
        }


        public static double CalculateAccuracy(int txPower, double rssi)
        {
            var accuracy = -1.0;
            if (rssi > 0)
            {
                var ratio = rssi * 1.0 / txPower;
                if (ratio < 1.0)
                {
                    accuracy = Math.Pow(ratio, 10);
                }
                else
                {
                    accuracy = 0.89976 * Math.Pow(ratio, 7.7095) + 0.111;
                }
            }
            return accuracy;
        }


        public static bool IsBeaconPacket(this byte[] data, bool skipManufacturerByte = true)
        {
            if (data == null)
                return false;

            if (data.Length != 23)
                return false;

            // apple manufacturerID - https://www.bluetooth.com/specifications/assigned-numbers/company-Identifiers
            if (!skipManufacturerByte && data[0] != 76)
                return false;

            return true;
        }


        public static byte[] ToBytes(this Guid guid)
        {
            var hex = guid
                .ToString()
                .Replace("-", String.Empty)
                .Replace("{", String.Empty)
                .Replace("}", String.Empty)
                .Replace(":", String.Empty)
                .Replace("-", String.Empty);

            var bytes = Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();

            return bytes;
        }
    }
}
