using System;
using CSharpFunctionalExtensions;

namespace ApplicationCore.Shared.Helpers
{
    public static class MathHelpers
    {
        public static Result<double> CalculateXProjection(double hypotenuse, double yProjection)
        {
            if (yProjection == 0)
                return hypotenuse;
            
            if (hypotenuse < yProjection)
                return Result.Failure<double>($"гипотенуза не может быть меньше катета");
                
            var xProjection = Math.Sqrt(hypotenuse * hypotenuse - yProjection * yProjection);
            return xProjection;
        }
    }
}