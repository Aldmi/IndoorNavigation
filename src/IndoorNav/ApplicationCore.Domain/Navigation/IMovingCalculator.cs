using System.Collections.Generic;
using ApplicationCore.Domain.Distance;
using ApplicationCore.Domain.Navigation.Model;

namespace ApplicationCore.Domain.Navigation
{
    /// <summary>
    /// Вычислитель перемещения.
    /// </summary>
    public interface IMovingCalculator
    {
        Moving CalculateMove(IEnumerable<BeaconDistanceModel> inputDataList);
    }
}