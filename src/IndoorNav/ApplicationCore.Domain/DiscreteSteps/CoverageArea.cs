namespace ApplicationCore.Domain.DiscreteSteps
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
        
        public override string ToString() => $"{Radius:F2}м";
    }
}