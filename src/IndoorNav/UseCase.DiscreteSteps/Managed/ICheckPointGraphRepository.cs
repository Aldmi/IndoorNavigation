using ApplicationCore.Domain.DiscreteSteps;
using ApplicationCore.Domain.DiscreteSteps.Model;
using ApplicationCore.Shared.DataStruct.GraphNotOriented;

namespace UseCase.DiscreteSteps.Managed
{
    public interface ICheckPointGraphRepository //TODO: вынести в слой Interfaces.DiscreteSteps или в упрощенном виде оставить тут
    {
        Graph<CheckPoint> GetGraph();
    }
}