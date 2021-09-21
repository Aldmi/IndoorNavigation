using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Domain.DistanceService.Services.DistanceFilters
{
    public class DistanceFilterRemoveDifferentValues : IDistanceFilter
    {
        public IEnumerable<double> Invoke(double? previousDistance, IEnumerable<double> distances)
        {
           var array= distances.ToArray();

           var res= array.Skip(1);
           return res;
        }
    }
}