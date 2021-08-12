using ApplicationCore.Domain.DiscreteSteps;

namespace UseCase.DiscreteSteps.Managed
{
    public interface ICheckPointGraphRepository //TODO: вынести в слой Interfaces.DiscreteSteps или в упрощенном виде оставить тут
    {
        CheckPointGraph GetGraph();
    }
}