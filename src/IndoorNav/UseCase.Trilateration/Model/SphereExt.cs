namespace UseCase.Trilateration.Model
{
    public static class SphereExt
    {
        public static SphereStatistic CreateStatisticForLogger(this Sphere sphere)
        {
            return new SphereStatistic();
        }
    }

    public class SphereStatistic
    {
        
    }
}