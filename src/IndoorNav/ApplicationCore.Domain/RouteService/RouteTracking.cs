using ApplicationCore.Domain.MovingService;
using ApplicationCore.Domain.RouteService.Model;

namespace ApplicationCore.Domain.RouteService
{
    /// <summary>
    /// Слежение за маршрутом.
    /// </summary>
    public class RouteTracking : IRouteTracking
    {
        public Route Route { get; private set;}


        
        public void SetRoute(Route route)
        {
            Route = route;
        }
        
        
        public Tracking Control(Moving moving)
        {
            
            //проверяем верно ли мы дыижемся по Routeж
            
            return new Tracking(TrackingState.OnRoute);
        }
        
    }
}