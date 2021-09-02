using System.Collections.Generic;
using ApplicationCore.Domain.DiscreteSteps.Model;

namespace ApplicationCore.Domain.Navigation.Model
{
    /// <summary>
    /// Маршрут
    /// </summary>
    public class Route
    {
        public Route(string name, IReadOnlyList<CheckPoint> listCheckPoints)
        {
            Name = name;
            ListCheckPoints = listCheckPoints;
        }

        public string Name { get; }
        public IReadOnlyList<CheckPoint> ListCheckPoints { get;}


        public CheckPoint GetStart() => ListCheckPoints[0];
        public CheckPoint GetEnd() => ListCheckPoints[^0];
    }
}