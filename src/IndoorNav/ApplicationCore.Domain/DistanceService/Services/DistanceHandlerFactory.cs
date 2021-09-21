using System.Collections.Generic;
using ApplicationCore.Domain.DistanceService.Services.DistanceAggregators;
using ApplicationCore.Domain.DistanceService.Services.DistanceFilters;

namespace ApplicationCore.Domain.DistanceService.Services
{
    public static class DistanceHandlerFactory
    {
        public static DistanceHandler CreateRemoveDifferentAndAverageAggergate()
        {
            return new DistanceHandler(
                new List<IDistanceFilter>
                {
                    new DistanceFilterRemoveDifferentValues()
                },
                new DistanceAggregatorsAverage());
        }
    }
}