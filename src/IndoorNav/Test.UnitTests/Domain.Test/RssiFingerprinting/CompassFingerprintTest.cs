using System;
using System.Collections;
using System.Collections.Generic;
using ApplicationCore.Domain.RssiFingerprinting.Model;
using ApplicationCore.Shared.Models;
using CSharpFunctionalExtensions;
using FluentAssertions;
using Libs.Beacons.Models;
using Xunit;

namespace Test.Beacons.Domain.Test.RssiFingerprinting
{
    public class CompassFingerprintTest
    {
        
        #region TheoryData
        static Guid uuid = Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e");
        static ushort major = 1;
        static int txPower = -59;
        public static IEnumerable<object[]> CreateSimilarDatas => new[]
        {
            new object[]
            {
                new CompassFingerprint(
                    new CompassCoordinates(60.0),
                    new List<BeaconAverage>
                    {
                        new(new BeaconId(uuid, major, 1), -65.6, txPower),
                        new(new BeaconId(uuid, major, 2), -85.1, txPower),
                        new(new BeaconId(uuid, major, 3), -76.8, txPower)
                    }),
                new CompassFingerprint(
                    new CompassCoordinates(60.0),
                    new List<BeaconAverage>
                    {
                        new(new BeaconId(uuid, major, 1), -60.6, txPower),
                        new(new BeaconId(uuid, major, 2), -66.1, txPower),
                        new(new BeaconId(uuid, major, 3), -78.8, txPower)
                    }),
                0,
                new List<SimilarCompassFingerprint.DifferenceBeaconAverage>()
                {
                    new(new BeaconId(uuid, major, 1), 4.99, 0.90),
                    new(new BeaconId(uuid, major, 1), 19.0, 13.0),
                    new(new BeaconId(uuid, major, 1), -2.0, -1.5)
                }
            },
            
            new object[]
            {
                new CompassFingerprint(
                    new CompassCoordinates(60.0),
                    new List<BeaconAverage>
                    {
                        new(new BeaconId(uuid, major, 1), -65.6, txPower),
                        new(new BeaconId(uuid, major, 2), -85.1, txPower),
                        new(new BeaconId(uuid, major, 3), -76.8, txPower),
                        new(new BeaconId(uuid, major, 4), -71.1, txPower)
                    }),
                new CompassFingerprint(
                    new CompassCoordinates(60.0),
                    new List<BeaconAverage>
                    {
                        new(new BeaconId(uuid, major, 1), -60.6, txPower),
                        new(new BeaconId(uuid, major, 2), -82.1, txPower)
                    }),
                2,
                new List<SimilarCompassFingerprint.DifferenceBeaconAverage>()
                {
                    new(new BeaconId(uuid, major, 1), 5.0, 0.90),
                    new(new BeaconId(uuid, major, 2), 3.0, 3.7),
                }
            },
        };
        #endregion
        /// <summary>
        /// Создание объекта показывающего похожесть сигнала на рефернсный.
        /// </summary>
        /// <param name="refCf"></param>
        /// <param name="cf"></param>
        /// <param name="expectedMissingFingerprintsCount"></param>
        /// <param name="expectedDiffList"></param>
        [Theory]
        [MemberData(nameof(CreateSimilarDatas))]
        public void CreateSimilarTest(CompassFingerprint refCf, CompassFingerprint cf, int expectedMissingFingerprintsCount, List<SimilarCompassFingerprint.DifferenceBeaconAverage> expectedDiffList)
        {
            //act
            var (isSuccess, _, similar, _) = refCf.CreateSimilar(cf);

            //assert
            isSuccess.Should().BeTrue();
            similar.MissingFingerprints.Count.Should().Be(expectedMissingFingerprintsCount);
            for (var i = 0; i < similar.DifferenceFingerprints.Count; i++)
            {
                var diff = similar.DifferenceFingerprints[i];
                var expectedDiff = expectedDiffList[i];
                diff.DeltaRssi.Should().BeApproximately(expectedDiff.DeltaRssi, 0.01);
                diff.DeltaDistance.Should().BeApproximately(expectedDiff.DeltaDistance, 0.01);
            }
        }

        
        #region TheoryData
        public static IEnumerable<object[]> CreateCompareDatas => new[]
        {
            // new object[]
            // {
            //     new CompassFingerprint(
            //         new CompassCoordinates(60.0),
            //         new List<BeaconAverage>
            //         {
            //             new(new BeaconId(uuid, major, 1), -65.6, txPower),
            //             new(new BeaconId(uuid, major, 2), -85.1, txPower),
            //             new(new BeaconId(uuid, major, 3), -76.8, txPower)
            //         }),
            //     new CompassFingerprint(
            //         new CompassCoordinates(60.0),
            //         new List<BeaconAverage>
            //         {
            //             new(new BeaconId(uuid, major, 1), -60.6, txPower),
            //             new(new BeaconId(uuid, major, 2), -74.1, txPower),
            //             new(new BeaconId(uuid, major, 3), -69.8, txPower)
            //         }),
            //     new CompassFingerprint(
            //         new CompassCoordinates(60.0),
            //         new List<BeaconAverage>
            //         {
            //             new(new BeaconId(uuid, major, 1), -67.6, txPower),
            //             new(new BeaconId(uuid, major, 2), -88.1, txPower),
            //             new(new BeaconId(uuid, major, 3), -77.8, txPower)
            //         }),
            //     //cf2 ближе к refCf чем cf1
            //     false,
            //     true
            // },
            
            // new object[]
            // {
            //     new CompassFingerprint(
            //         new CompassCoordinates(60.0),
            //         new List<BeaconAverage>
            //         {
            //             new(new BeaconId(uuid, major, 1), -65.6, txPower),
            //             new(new BeaconId(uuid, major, 2), -85.1, txPower),
            //             new(new BeaconId(uuid, major, 3), -76.8, txPower)
            //         }),
            //     new CompassFingerprint(
            //         new CompassCoordinates(60.0),
            //         new List<BeaconAverage>
            //         {
            //             new(new BeaconId(uuid, major, 1), -68.6, txPower),
            //             new(new BeaconId(uuid, major, 2), -88.1, txPower),
            //             new(new BeaconId(uuid, major, 3), -70.8, txPower)
            //         }),
            //     new CompassFingerprint(
            //         new CompassCoordinates(60.0),
            //         new List<BeaconAverage>
            //         {
            //             new(new BeaconId(uuid, major, 1), -70.6, txPower),
            //             new(new BeaconId(uuid, major, 2), -89.1, txPower),
            //             new(new BeaconId(uuid, major, 3), -77.8, txPower)
            //         }),
            //     //cf1 ближе к refCf чем cf2
            //     true,
            //     true
            // },
            
            // new object[]
            // {
            //     new CompassFingerprint(
            //         new CompassCoordinates(60.0),
            //         new List<BeaconAverage>
            //         {
            //             new(new BeaconId(uuid, major, 1), -65.6, txPower),
            //             new(new BeaconId(uuid, major, 2), -85.1, txPower),
            //             new(new BeaconId(uuid, major, 3), -76.8, txPower)
            //         }),
            //     new CompassFingerprint(
            //         new CompassCoordinates(60.0),
            //         new List<BeaconAverage>
            //         {
            //             new(new BeaconId(uuid, major, 1), -68.6, txPower),
            //             new(new BeaconId(uuid, major, 2), -88.1, txPower),
            //             new(new BeaconId(uuid, major, 3), -70.8, txPower)
            //         }),
            //     new CompassFingerprint(
            //         new CompassCoordinates(60.0),
            //         //Отпечаток лучтше, хоть и сигналов меньше.
            //         new List<BeaconAverage>
            //         {
            //             new(new BeaconId(uuid, major, 1), -66.6, txPower),
            //             new(new BeaconId(uuid, major, 2), -84.9, txPower),
            //         }),
            //     false,
            //     false
            // },
            
            
            new object[]
            {
                new CompassFingerprint(
                    new CompassCoordinates(60.0),
                    new List<BeaconAverage>
                    {
                        new(new BeaconId(uuid, major, 1), -65.6, txPower),
                        new(new BeaconId(uuid, major, 2), -85.1, txPower),
                    }),
                new CompassFingerprint(
                    new CompassCoordinates(60.0),
                    new List<BeaconAverage>
                    {
                        //в референсном отпечатке 2 сигнала, поэтому сравниваем только по ним (отбрасываем 2-а последних)
                        new(new BeaconId(uuid, major, 1), -68.6, txPower),
                        new(new BeaconId(uuid, major, 2), -88.1, txPower),
                        new(new BeaconId(uuid, major, 3), -70.8, txPower),
                        new(new BeaconId(uuid, major, 4), -70.8, txPower)
                    }),
                new CompassFingerprint(
                    new CompassCoordinates(60.0),
                    new List<BeaconAverage>
                    {
                        new(new BeaconId(uuid, major, 1), -66.6, txPower),
                        new(new BeaconId(uuid, major, 2), -84.9, txPower),
                    }),
                false,
                false
            },
        };
        #endregion

        /// <summary>
        /// Какой из отпечатков cf1 или cf2 наиболее похож на рефернсный refCf. SmallestDeltaRssi
        /// Какой из отпечатков cf1 или cf2 лучтше по сигналу rssi относительно рефернсного refCf. BetterDeltaRssi
        /// </summary> 
        /// <param name="refCf"></param>
        /// <param name="cf1"></param>
        /// <param name="cf2"></param>
        /// <param name="expectedSmallestDeltaRsiiCmpRes">true- cf1 ближе к рефернсному, false- cf2 ближе к рефернсному.</param>
        /// <param name="expectedBetterDeltaRsiiCmpRes">true- cf1 лутчше относительно рефернсного, false- cf2 лутчше относительно рефернсного.</param>
        [Theory]
        [MemberData(nameof(CreateCompareDatas))]
        public void CmpDeltaRssiRelativeReference(CompassFingerprint refCf, CompassFingerprint cf1, CompassFingerprint cf2, bool expectedSmallestDeltaRsiiCmpRes, bool expectedBetterDeltaRsiiCmpRes)
        {
            //arrange
            var (_, _, similar1, _) = refCf.CreateSimilar(cf1);
            var (_, _, similar2, _) = refCf.CreateSimilar(cf2);
            
            //act
            var cmpSmallestDeltaRssi = similar1.SmallestDeltaRssi(similar2); 
            //var cmpBetterDeltaRssi = similar1.BetterDeltaRssi(similar2); 
            
            //assert
            cmpSmallestDeltaRssi.Should().Be(expectedSmallestDeltaRsiiCmpRes);
            //cmpBetterDeltaRssi.Should().Be(expectedBetterDeltaRsiiCmpRes);
        }
    }
}