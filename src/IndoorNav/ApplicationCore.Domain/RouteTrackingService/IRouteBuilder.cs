using System;
using ApplicationCore.Domain.CheckPointModel;
using ApplicationCore.Domain.RouteTrackingService.Model;

namespace ApplicationCore.Domain.RouteTrackingService
{
    /// <summary>
    /// Построитель маршрута.
    /// </summary>
    public interface IRouteBuilder
    {
        public Route Build(string name, CheckPointBase startCh, CheckPointBase endCh);
        IObservable<Route> BuildRouteRx { get; }
    }
}