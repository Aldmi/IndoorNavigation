using System.Collections.Generic;
using ApplicationCore.Domain.MovingService.DiscreteSteps.Model;

namespace ApplicationCore.Domain.RouteService.Model
{
    /// <summary>
    /// Маршрут
    /// </summary>
    public class Route
    {
        public Route(string name, IReadOnlyList<CheckPointDs> listCheckPoints)
        {
            Name = name;
            ListCheckPoints = listCheckPoints;
        }

        public string Name { get; }
        public IReadOnlyList<CheckPointDs> ListCheckPoints { get;}


        public CheckPointDs GetStart() => ListCheckPoints[0];
        public CheckPointDs GetEnd() => ListCheckPoints[^0];
    }
}