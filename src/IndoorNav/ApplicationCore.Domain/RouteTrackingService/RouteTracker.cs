using ApplicationCore.Domain.MovingService.Model;
using ApplicationCore.Domain.RouteTrackingService.Model;

namespace ApplicationCore.Domain.RouteTrackingService
{
    /// <summary>
    /// Слежение за маршрутом.
    /// </summary>
    public class RouteTracker : IRouteTracker
    {
        public Route Route { get; private set;}
        public TrackingResult CurrentTrackingResult { get; private set;}
        
        public void SetRoute(Route route)
        {
            Route = route;
        }
        
        
        public TrackingResult Control(Moving moving)
        {
            /*
                проверяем верно ли мы дыижемся по Route
                
                если конечная точка маршрута (CompleteRoute) то на стороне подписчика нужно 
                .TakeWhile(x=>x!=CompleteRoute);
                 тогда закончится генерация последовательности.
                 
                 
                 CompleteRoute нужно давать после выдачи последнего удачного Tracking(OnRoute)
            */
            
            return new TrackingResult(TrackingState.OnRoute);
        }
        
    }
}