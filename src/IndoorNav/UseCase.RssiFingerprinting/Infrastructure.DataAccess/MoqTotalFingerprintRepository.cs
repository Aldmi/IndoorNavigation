using System;
using System.Collections.Generic;
using ApplicationCore.Domain.RssiFingerprinting.Model;
using ApplicationCore.Shared.Models;
using Libs.Beacons.Models;

namespace UseCase.RssiFingerprinting.Infrastructure.DataAccess
{
    public class MoqTotalFingerprintRepository : ITotalFingerprintRepository
    {
        public List<TotalFingerprint> GetTotalFingerprintList()
        {
            var uuid = Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e");
            ushort major = 1;
            var txPower = -59;
            var list = new List<TotalFingerprint>()
            {
                //маска точки 1,1
                new TotalFingerprint(
                   Guid.NewGuid(),
                   new Point(1,1),
                   new Dictionary<CompassCoordinates, CompassFingerprint>
                   {
                       //3 отпечатка на север (60.0)
                       {new CompassCoordinates(60.0), new CompassFingerprint(new CompassCoordinates(60.0),
                           new List<BeaconAverage> {
                               new BeaconAverage(new BeaconId(uuid, major, 1), -65.6, -txPower),
                               new BeaconAverage(new BeaconId(uuid, major, 2), -85.1, -txPower),
                               new BeaconAverage(new BeaconId(uuid, major, 3), -77.8, -txPower)
                           })
                       },
                       //3 отпечатка на юг (180.0)
                       {new CompassCoordinates(180.0), new CompassFingerprint(new CompassCoordinates(180.0),
                           new List<BeaconAverage> {
                               new BeaconAverage(new BeaconId(uuid, major, 1), -89.6, -txPower),
                               new BeaconAverage(new BeaconId(uuid, major, 2), -60.1, -txPower),
                               new BeaconAverage(new BeaconId(uuid, major, 3), -76.8, -txPower)
                           })
                       }
                   }),
                //маска точки 1,3
                new TotalFingerprint(
                    Guid.NewGuid(),
                    new Point(1,3),
                    new Dictionary<CompassCoordinates, CompassFingerprint>
                    {
                        //3 отпечатка на север (60.0)
                        {new CompassCoordinates(60.0), new CompassFingerprint(new CompassCoordinates(60.0),
                            new List<BeaconAverage> {
                                new BeaconAverage(new BeaconId(uuid, major, 1), -80.6, -txPower),
                                new BeaconAverage(new BeaconId(uuid, major, 2), -60.1, -txPower),
                                new BeaconAverage(new BeaconId(uuid, major, 3), -71.8, -txPower)
                            })
                        },
                        //3 отпечатка на юг (180.0)
                        {new CompassCoordinates(180.0), new CompassFingerprint(new CompassCoordinates(180.0),
                            new List<BeaconAverage> {
                                new BeaconAverage(new BeaconId(uuid, major, 1), -81.6, -txPower),
                                new BeaconAverage(new BeaconId(uuid, major, 2), -77.1, -txPower),
                                new BeaconAverage(new BeaconId(uuid, major, 3), -90.8, -txPower)
                            })
                        }
                    })
            };

            return list;
        }
    }
}