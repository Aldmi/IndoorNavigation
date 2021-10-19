using System;
using System.Collections.Generic;
using ApplicationCore.Domain.CheckPointModel;
using ApplicationCore.Domain.DistanceService.Model;
using ApplicationCore.Domain.MovingService.Model;

namespace ApplicationCore.Domain.MovingService
{
    /// <summary>
    /// Вычислитель перемещения.
    /// </summary>
    public interface IGraphMovingCalculator
    {
        CheckPointBase? CurrentCheckPoint { get; }
        Guid SharedUuid { get; }

        Moving CalculateMove(IEnumerable<BeaconDistance> inputDataList);
        void Reset();
    }
}