using System;
using CSharpFunctionalExtensions;

namespace ApplicationCore.Domain.DistanceService
{
    
    public static class DistanceXProjectionCalculator
    {
        public static Result<double> CalculateXProjection(double hypotenuse, double yProjection)
        {
            if (hypotenuse < yProjection)
                return Result.Failure<double>($"гипотенуза не может быть меньше катета");
                
            var xProjection = Math.Sqrt(hypotenuse * hypotenuse - yProjection * yProjection);
            return xProjection;
        }
    }
}