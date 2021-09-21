using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Domain.DistanceService.Services.DistanceAggregators
{
    public class DistanceAggregatorsAverage : IDistanceAggregator
    {
        public double Invoke(IEnumerable<double> distances)
        {
            return distances.Average();
        }
    }
}