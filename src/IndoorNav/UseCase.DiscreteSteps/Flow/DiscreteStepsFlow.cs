using System;
using System.Collections.Generic;
using ApplicationCore.Domain;
using ApplicationCore.Domain.Trilateration.Flows;
using ApplicationCore.Domain.Trilateration.Spheres;
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
        /// <param name="bufferTime"></param>
        /// <param name="sphereFactory"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static IObservable<IList<Sphere>> ManagedScanFlow(this IObservable<Beacon> sourse,
            TimeSpan bufferTime,
            SphereFactory sphereFactory,
            ILogger? logger = null)
        {
            return sourse
                //Буфферизация и разбиение на группы по Id
                .GroupAfterBuffer(bufferTime)
                //Создание сфер
                .CreateSphere(sphereFactory);
        }
    }
}