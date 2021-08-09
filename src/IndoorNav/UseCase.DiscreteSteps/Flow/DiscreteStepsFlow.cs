﻿using System;
using System.Collections.Generic;
using ApplicationCore.Domain;
using ApplicationCore.Domain.Flows;
using ApplicationCore.Domain.Spheres;
using Libs.Beacons.Flows;
using Libs.Beacons.Models;
using Microsoft.Extensions.Logging;

namespace UseCase.DiscreteSteps.Flow
{
    public static class DiscreteStepsFlow
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
                //Буфферизация и разбиение на группы по Id
                .GroupAfterBuffer(bufferTime)
                //Создание сфер
                .CreateSphere(sphereFactory);
        }
    }
}