using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using ApplicationCore.Domain;
using ApplicationCore.Domain.DiscreteSteps;
using ApplicationCore.Domain.DiscreteSteps.Flows;
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
        /// Конструктор потока Moving для сервиса ManagedScan.
        /// </summary>
        /// <param name="sourse"></param>
        /// <param name="bufferTime">время накопления данных по группам</param>
        /// <param name="txPower">мощность маяка на расстоянии 1м</param>
        /// <param name="calculateMove"> Сервис реализующий calculateMove, должен быть StateFull чтобы хранить текущее положенгие в графе и вычислять Moving</param>
        /// <param name="logger"></param>
        /// <returns>Текушее положение объекта в графе.</returns>
        public static IObservable<Moving> ManagedScanDiscreteStepsFlow(this IObservable<Beacon> sourse,
            TimeSpan bufferTime,
            int txPower,
            Func<IEnumerable<InputData>, Moving> calculateMove,
            ILogger? logger = null)
        {
            return sourse
                //Буфферизация и разбиение на группы по Id
                .GroupAfterBuffer(bufferTime)
                //Маппинг к InData. (фильтрация значений в группе и преобразование к InData)
                .Map2InData(txPower)
                //Упорядочить по Distance
                .OrderByDescendingForRange()
                //Определить перемещение в графе движения, используя функцию calculateMove.
                .Select(listInData=> calculateMove(listInData));
        }
    }
}