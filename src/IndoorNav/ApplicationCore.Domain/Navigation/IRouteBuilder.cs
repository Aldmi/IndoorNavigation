using System.Collections;
using System.Collections.Generic;
using ApplicationCore.Domain.DiscreteSteps.Model;
using ApplicationCore.Domain.Navigation.Model;

namespace ApplicationCore.Domain.Navigation
{
    /// <summary>
    /// Построитель маршрута.
    /// </summary>
    public interface IRouteBuilder
    {
        public Route Build(CheckPoint startCh, CheckPoint endCh);
    }
}