using ApplicationCore.Domain.MovingService.Model;
using ApplicationCore.Domain.RouteTrackingService.Model;

namespace ApplicationCore.Domain.RouteTrackingService
{
    /// <summary>
    /// Слежение за маршрутом.
    /// </summary>
    public interface IRouteTracker
    {
        Route Route { get;}
        TrackingResult CurrentTrackingResult { get;}
        void SetRoute(Route route);
        public TrackingResult Control(Moving moving);
    }
}