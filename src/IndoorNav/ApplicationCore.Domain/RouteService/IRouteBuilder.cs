using ApplicationCore.Domain.MovingService.DiscreteSteps.Model;
using ApplicationCore.Domain.RouteService.Model;

namespace ApplicationCore.Domain.RouteService
{
    /// <summary>
    /// Построитель маршрута.
    /// </summary>
    public interface IRouteBuilder
    {
        public Route Build(CheckPointDs startCh, CheckPointDs endCh);
    }
}