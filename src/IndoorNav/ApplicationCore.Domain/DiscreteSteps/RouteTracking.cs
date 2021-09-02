using System.Collections;
using System.Collections.Generic;
using ApplicationCore.Domain.DiscreteSteps.Model;
using ApplicationCore.Domain.Navigation;
using ApplicationCore.Domain.Navigation.Model;

namespace ApplicationCore.Domain.DiscreteSteps
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