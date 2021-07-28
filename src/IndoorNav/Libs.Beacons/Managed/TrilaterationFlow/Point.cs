namespace Libs.Beacons.Managed.TrilaterationFlow
{
    //TODO: Наследоваться от ValueObj
    public class Point
    {
        public double X { get; }
        public double Y { get; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
        
        public override string ToString() => $"({X}:{Y})";
    }
}