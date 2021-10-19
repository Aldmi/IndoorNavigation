using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using ApplicationCore.Domain.DistanceService.Filters;
using ApplicationCore.Domain.RssiFingerprinting.Model;
using ApplicationCore.Shared.Helpers;
using ApplicationCore.Shared.Models;
using CSharpFunctionalExtensions;
using Libs.Beacons.Flows;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.RssiFingerprinting
{
    public static class BeaconLocalFingerprintFlow
    {
        public static IObservable<TotalFingerprint> Beacon2CompassFingerprint(this IObservable<Beacon> sourse,
            TimeSpan bufferTime,
            KalmanBeaconDistanceFilter? kalmanDistanceFilter,
            double maxDistance)
        {
            var flow = sourse
                //Буфферизация и разбиение на группы по Id
                .GroupAfterBuffer(bufferTime)
                //Среднее из сигналов Rssi для каждого датчика
                .CalcAverageRssiInGroupBeacons(RssiHelpers.CalculateAverageRssi)
                //Убрать маленькие AverageRssi (пересчитанные к Distance)
                .RemoveSmallBeaconAverage(maxDistance)
                //AverageRssi -> RssiFingerprint
                .MapBeaconAverage2CompassFingerprint()
                //Найти похожий отпечаток в БД среди TotalFingerprint.
                .FindSimilarFingerprint();
                
                
            
            return flow;
        }


        
        public static IObservable<IList<BeaconAverage>> RemoveSmallBeaconAverage(this IObservable<IList<BeaconAverage>> sourse, double maxDistance)
        {
            return sourse.Select(list =>
            {
                return list
                    .Where(ba =>
                    {
                        var (_, isFailure, distance, _) = RssiHelpers.CalculateDistance(ba.TxPower, ba.Rssi);
                        
                         if (isFailure)
                             return false;
                            
                        return distance <= maxDistance;
                    })
                    .ToList();
            });

        }
        
        /// <summary>
        /// Список сигналов от маяков (BeaconAverage) и координаты компаса, обернуть в CompassFingerprint.
        /// </summary>
        public static IObservable<CompassFingerprint> MapBeaconAverage2CompassFingerprint(this IObservable<IList<BeaconAverage>> sourse)
        {
            
            var compassCoordinates = new CompassCoordinates(); //TODO: получать с сервиса компаса координаты.
            
            
            return sourse.Select(listBeaconAverage =>new CompassFingerprint(compassCoordinates, listBeaconAverage));
        }
        
        
        /// <summary>
        /// Найти наиболее похожий отпечаток из коллекции отпечатков TotalFingerprint.
        /// </summary>
        public static IObservable<TotalFingerprint> FindSimilarFingerprint(this IObservable<CompassFingerprint> sourse)
        {
            return sourse.Select(fingerprints =>
            {
                //Поиск нужного отпечатка в БД

                var similarFingerprint= new TotalFingerprint(new Point(1.0, 1.0), new List<CompassFingerprint>()); //TODO: выполнять поиск в БД, через репозиторий TotalFingerprintRepository
                return similarFingerprint;
            });

        }
    }
}