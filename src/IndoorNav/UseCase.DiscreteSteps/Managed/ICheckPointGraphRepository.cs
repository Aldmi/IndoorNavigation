using System;
using ApplicationCore.Domain.CheckPointModel;
using ApplicationCore.Shared.DataStruct.GraphNotOriented;

namespace UseCase.DiscreteSteps.Managed
{
    public interface ICheckPointGraphRepository //TODO: вынести в слой Interfaces.DiscreteSteps или в упрощенном виде оставить тут
    {
        Graph<CheckPointBase> GetGraph();
        public Guid GetSharedUuid();
    }
}