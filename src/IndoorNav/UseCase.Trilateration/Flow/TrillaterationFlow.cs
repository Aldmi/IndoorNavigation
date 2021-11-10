using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using ApplicationCore.Domain;
using ApplicationCore.Domain.CheckPointModel.Trilateration.Spheres;
using Libs.Beacons.Flows;
using Libs.Beacons.Models;
using Microsoft.Extensions.Logging;

namespace UseCase.Trilateration.Flow
{
    public static class TrillaterationFlow
    {
        /// <summary>
        /// Конструктор потока для сервиса ManagedScan.
        /// </summary>
        /// <param name="sourse"></param>
        /// <param name="whiteList"></param>
        /// <param name="bufferTime"></param>
        /// <param name="sphereFactory"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static IObservable<IList<Sphere>> ManagedScanFlow(this IObservable<Beacon> sourse,
            IEnumerable<BeaconId> whiteList,
            TimeSpan bufferTime,
            SphereFactory sphereFactory,
            ILogger? logger = null)
        {
            return sourse
                //Проходят только значения из списка
                .WhenWhiteList(whiteList)
                
                //TODO: убрать все что ниже и использовать BeaconDistanceFlow(), и потом на базе этих данных создавать Flow.
                //Буфферизация и разбиение на группы по BeaconId
                .GroupAfterBuffer(bufferTime)
                //Создание сфер
                .CreateSphere(sphereFactory);                
        }
    }
}
