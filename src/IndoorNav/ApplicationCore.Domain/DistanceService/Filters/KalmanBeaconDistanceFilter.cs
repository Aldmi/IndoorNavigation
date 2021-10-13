using ApplicationCore.Domain.DistanceService.Model;
using ApplicationCore.Shared.Services;

namespace ApplicationCore.Domain.DistanceService.Filters
{
    /// <summary>
    /// Фильтр Калмана для double значения расстояния до Beacon метки. 
    /// </summary>
    public class KalmanBeaconDistanceFilter
    {
        private readonly double _q;
        private readonly double _r;
        private readonly double _covariance;
        private readonly double _f;
        private readonly double _h;

        public KalmanBeaconDistanceFilter(double q, double r, double covariance, double f = 1, double h = 1)
        {
            _q = q;
            _r = r;
            _covariance = covariance;
            _f = f;
            _h = h;
        }


        public BeaconDistance Filtred(BeaconDistance data)
        {
            return data;
        }
        
    }
}