using System.Collections.Generic;

namespace ApplicationCore.Domain.DistanceService.Services
{
    public interface IDistanceAggregator
    {
        double Invoke(IEnumerable<double> distances);
    }
}