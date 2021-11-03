using System;
using System.Collections.Generic;
using ApplicationCore.Domain.RssiFingerprinting.Model;
using ApplicationCore.Shared.Models;
using Libs.Beacons.Models;

namespace Test.Beacons.Domain.Test.RssiFingerprinting.Data
{
    public static class TotalFingerprintDatas
    {
        static Guid uuid = Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e");
        static ushort major = 1;
        static int txPower = -59;
        

        /// <summary>
        /// Создать BeaconAverage
        /// </summary>
        public static BeaconAverage CreateBeaconAverage(ushort minor, double rssi)
            => new(new BeaconId(uuid, major, minor), rssi, txPower);
        
        
        /// <summary>
        /// 3 отпечатка Point меняется только по Y.
        /// </summary>
        public static IEnumerable<TotalFingerprint> GetTotalFingerprint_3TFp_Linear_on_Y_moving_By_North()
        {
            return new List<TotalFingerprint>
            {
                //0m 
                new(
                    Guid.NewGuid(), 
                    new Point(0, 0),
                    new Dictionary<CompassCoordinates, CompassFingerprint>
                    {
                        {
                            CompassCoordinates.North,
                            new CompassFingerprint(CompassCoordinates.North, new List<BeaconAverage>
                            {
                                CreateBeaconAverage(1, -56.0),
                                CreateBeaconAverage(2, -74.0),
                                CreateBeaconAverage(3, -80.0),
                                CreateBeaconAverage(4, -88.0)
                            })
                        }
                    }),
                //7.5m
                new(
                    Guid.NewGuid(),
                    new Point(0, 7.5),
                    new Dictionary<CompassCoordinates, CompassFingerprint>
                    {
                        {
                            CompassCoordinates.North,
                            new CompassFingerprint(CompassCoordinates.North, new List<BeaconAverage>
                            {
                                CreateBeaconAverage(1, -78.0),
                                CreateBeaconAverage(2, -67.0),
                                CreateBeaconAverage(3, -67.0),
                                CreateBeaconAverage(4, -78.0)
                            })
                        }
                    }),
                //12.5m
                new(
                    Guid.NewGuid(),
                    new Point(0, 12.5),
                    new Dictionary<CompassCoordinates, CompassFingerprint>
                    {
                        {
                            CompassCoordinates.North,
                            new CompassFingerprint(CompassCoordinates.North, new List<BeaconAverage>
                            {
                                CreateBeaconAverage(1, -86.0),
                                CreateBeaconAverage(2, -78.0),
                                CreateBeaconAverage(3, -67.0),
                                CreateBeaconAverage(4, -67.0)
                            })
                        }
                    })
            };
        }
        
        
        /// <summary>
        /// 3 отпечатка Point меняется только по Y.
        /// </summary>
        public static IEnumerable<TotalFingerprint> GetTotalFingerprint_3Fp_Linear_on_Y_moving()
        {
            return new List<TotalFingerprint>
            {
                new(
                    Guid.NewGuid(), 
                    new Point(1, 1),
                    new Dictionary<CompassCoordinates, CompassFingerprint>
                    {
                        {
                            CompassCoordinates.North,
                            new CompassFingerprint(CompassCoordinates.North, new List<BeaconAverage>
                            {
                                CreateBeaconAverage(1, -65.6),
                                CreateBeaconAverage(2, -81.1),
                                CreateBeaconAverage(3, -76.8)
                            })
                        },
                        {
                            CompassCoordinates.South,
                            new CompassFingerprint(CompassCoordinates.South, new List<BeaconAverage>
                            {
                                CreateBeaconAverage(1, -74.6),
                                CreateBeaconAverage(2, -88.1),
                                CreateBeaconAverage(3, -70.8)
                            })
                        }
                    }),
                new(
                    Guid.NewGuid(),
                    new Point(1, 3),
                    new Dictionary<CompassCoordinates, CompassFingerprint>
                    {
                        {
                            CompassCoordinates.North,
                            new CompassFingerprint(CompassCoordinates.North, new List<BeaconAverage>
                            {
                                CreateBeaconAverage(1, -70.6),
                                CreateBeaconAverage(2, -89.1),
                                CreateBeaconAverage(3, -78.8),
                                CreateBeaconAverage(4, -60.8)
                            })
                        },
                        {
                            CompassCoordinates.South,
                            new CompassFingerprint(CompassCoordinates.South, new List<BeaconAverage>
                            {
                                CreateBeaconAverage(1, -72.6),
                                CreateBeaconAverage(2, -87.1),
                                CreateBeaconAverage(3, -76.8),
                                CreateBeaconAverage(4, -65.8)
                            })
                        }
                    }),
                new(
                    Guid.NewGuid(),
                    new Point(1, 6),
                    new Dictionary<CompassCoordinates, CompassFingerprint>
                    {
                        {
                            CompassCoordinates.North,
                            new CompassFingerprint(CompassCoordinates.North, new List<BeaconAverage>
                            {
                                CreateBeaconAverage(1, -68.6),
                                CreateBeaconAverage(4, -69.8)
                            })
                        },
                        {
                            CompassCoordinates.South,
                            new CompassFingerprint(CompassCoordinates.South, new List<BeaconAverage>
                            {
                                CreateBeaconAverage(1, -72.6),
                                CreateBeaconAverage(2, -87.1),
                                CreateBeaconAverage(3, -76.8),
                                CreateBeaconAverage(4, -65.8)
                            })
                        }
                    })
            };
        }
    }
}