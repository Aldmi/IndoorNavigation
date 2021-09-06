using System.Collections.Generic;
using ApplicationCore.Domain.DistanceService;
using ApplicationCore.Domain.DistanceService.Model;
using ApplicationCore.Domain.MovingService.Model;

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