﻿using ApplicationCore.Domain.MovingService;
using ApplicationCore.Domain.MovingService.Model;
using ApplicationCore.Domain.RouteService.Model;

namespace ApplicationCore.Domain.RouteService
{
    /// <summary>
    /// Слежение за маршрутом.
    /// </summary>
    public interface IRouteTracker
    {
        Route Route { get;}
        Tracking CurrentTracking { get;}
        void SetRoute(Route route);
        public Tracking Control(Moving moving);
    }
}