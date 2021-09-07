using ApplicationCore.Domain.MovingService;
using ApplicationCore.Domain.MovingService.Model;
using ApplicationCore.Domain.RouteService.Model;

namespace ApplicationCore.Domain.RouteService
{
    /// <summary>
    /// Слежение за маршрутом.
    /// </summary>
    public class RouteTracker : IRouteTracker
    {
        public Route Route { get; private set;}
        public Tracking CurrentTracking { get; private set;}
        
        public void SetRoute(Route route)
        {
            Route = route;
        }
        
        
        public Tracking Control(Moving moving)
        {
            /*
                проверяем верно ли мы дыижемся по Route
                
                если конечная точка маршрута (CompleteRoute) то на стороне подписчика нужно 
                .TakeWhile(x=>x!=CompleteRoute);
                 тогда закончится генерация последовательности.
                 
                 
                 CompleteRoute нужно давать после выдачи последнего удачного Tracking(OnRoute)
            */
            
            return new Tracking(TrackingState.OnRoute);
        }
        
    }
}