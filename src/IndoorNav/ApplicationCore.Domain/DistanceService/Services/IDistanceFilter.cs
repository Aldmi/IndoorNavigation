using System.Collections.Generic;

namespace ApplicationCore.Domain.DistanceService.Services
{
    public interface IDistanceFilter
    {
        IEnumerable<double> Invoke(double? previousDistance, IEnumerable<double> distances);
    }
}