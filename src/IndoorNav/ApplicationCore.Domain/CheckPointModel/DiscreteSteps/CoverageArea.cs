using ApplicationCore.Shared.Models;

namespace ApplicationCore.Domain.CheckPointModel.DiscreteSteps
{
    /// <summary>
    /// Зона охвата маяка
    /// </summary>
    public class CoverageArea
    {
        public CoverageArea(double radius)
        {
            Radius = radius;
        }

        public double Radius { get; }


        public Zone GetZone(double realRange)
        {
            return Radius > realRange ? Zone.In : Zone.Out;
        }
        
        public override string ToString() => $"{Radius:F2}м";
    }
}