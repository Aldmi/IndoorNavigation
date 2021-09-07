using System.Collections.Generic;
using ApplicationCore.Domain.CheckPointModel;
using ApplicationCore.Domain.MovingService.DiscreteSteps.Model;

namespace ApplicationCore.Domain.RouteService.Model
{
    /// <summary>
    /// Маршрут
    /// </summary>
    public class Route
    {
        public Route(string name, IReadOnlyList<CheckPointBase> listCheckPoints)
        {
            Name = name;
            ListCheckPoints = listCheckPoints;
        }

        public string Name { get; }
        public IReadOnlyList<CheckPointBase> ListCheckPoints { get;}


        public CheckPointBase GetStart() => ListCheckPoints[0];
        public CheckPointBase GetEnd() => ListCheckPoints[^0];
    }
}