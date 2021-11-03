using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using ApplicationCore.Domain.RssiFingerprinting.Model;
using ApplicationCore.Domain.RssiFingerprinting.Services;
using ApplicationCore.Shared.Helpers;
using ApplicationCore.Shared.Models;
using CSharpFunctionalExtensions;
using Libs.Beacons.Flows;
using Libs.Beacons.Models;

namespace UseCase.RssiFingerprinting.Flows
{
    public static class TotalFingerprintFlow
    {
        public static IObservable<Result<TotalFingerprint>> Beacon2TotalFingerprint(this IObservable<Beacon> sourse,
            TimeSpan bufferTime,
            //KalmanBeaconDistanceFilter? kalmanDistanceFilter,
            IEnumerable<TotalFingerprint> totalList,
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
                .FindSimilarFingerprint(totalList);
                
            
            return flow;
        }
        
        private static IObservable<IList<BeaconAverage>> RemoveSmallBeaconAverage(this IObservable<IList<BeaconAverage>> sourse, double maxDistance)
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
        private static IObservable<CompassFingerprint> MapBeaconAverage2CompassFingerprint(this IObservable<IList<BeaconAverage>> sourse)
        {
            var compassCoordinates = CompassCoordinates.North; //TODO: получать с сервиса компаса координаты.
            return sourse.Select(listBeaconAverage =>new CompassFingerprint(compassCoordinates, listBeaconAverage));
        }
        
        
        /// <summary>
        /// Найти наиболее похожий отпечаток из коллекции отпечатков TotalFingerprint.
        /// </summary>
        private static IObservable<Result<TotalFingerprint>> FindSimilarFingerprint(this IObservable<CompassFingerprint> sourse, IEnumerable<TotalFingerprint> totalList)
        {
            return sourse.Select(cf =>
            {
                //Поиск нужного отпечатка в БД
                var res= FindSimilarTotalFingerprintService.FindTotalFingerprint(totalList, cf);
                return res.Map(s=>s.tf);
            });
        }
    }
}