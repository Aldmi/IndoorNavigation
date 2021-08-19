using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using ApplicationCore.Domain.Services;
using ApplicationCore.Shared;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.DiscreteSteps
{
    public static class InputDataFlow
    {

        /// <summary>
        /// Упорядочить список InputData по Distance (в порядке убывания).
        /// </summary>
        public static IObservable<IList<BeaconDistanceModel>> OrderByDescendingForDistance(this IObservable<IList<BeaconDistanceModel>> sourse)
        {
            return sourse
                .Select(list =>
                {
                    return list.OrderByDescending(inData => inData.Distance).ToList();
                });
        }
    }
}