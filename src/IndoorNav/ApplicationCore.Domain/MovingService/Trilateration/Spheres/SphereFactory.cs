using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Domain.Options;
using ApplicationCore.Shared;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.MovingService.Trilateration.Spheres
{
    public class SphereFactory
    {
        private readonly Func<int, int, double> _rssi2RangeConverter;
        private readonly IEnumerable<BeaconOption> _beaconOptions;

        public SphereFactory(
            Func<int, int, double> rssi2RangeConverter,
            IEnumerable<BeaconOption> beaconOptions
        )
        {
            _rssi2RangeConverter = rssi2RangeConverter;
            _beaconOptions = beaconOptions;
        }


        public Sphere Create(BeaconId beaconId, IEnumerable<Beacon> beacons)
        {
            try
            {
                var option = _beaconOptions.FirstOrDefault(o => o.EqualById(beaconId));
                var center = option?.Ancore ?? Point.EmptyPoint;
                var ranges=beacons
                    .Select(b =>  RangeBle.Create(b.Rssi, option?.TxPower ?? b.TxPower, _rssi2RangeConverter))
                    .ToList();
                var sphere = new Sphere(beaconId, center, ranges);
                return sphere;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}