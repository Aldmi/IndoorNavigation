using System.Collections.Generic;
using ApplicationCore.Domain.DistanceService;

namespace ApplicationCore.Domain.MovingService
{
    /// <summary>
    /// Вычислитель перемещения.
    /// </summary>
    public interface IMovingCalculator
    {
        Moving CalculateMove(IEnumerable<BeaconDistance> inputDataList);
    }
}