using System;
using CSharpFunctionalExtensions;

namespace ApplicationCore.Domain.DistanceService
{
    public static class Rssi2DistanceConverter
    {

        /// <summary>
        /// Вычислить расстояние по силе сигнала.
        /// Округляет вычисления до 1 знака после запятой
        /// </summary>
        /// <param name="measuredPower"></param>
        /// <param name="rssi"></param>
        /// <returns></returns>
        public static double CalculateDistance(int measuredPower, int rssi)
        {
            double distance;
            if (rssi == 0) {
                return -1.0; // if we cannot determine distance, return -1.
            }
            double ratio = rssi*1.0/measuredPower;
            if (ratio < 1.0) 
            {
                distance= Math.Pow(ratio,10);
            }
            else
            {
                distance =  (0.89976)*Math.Pow(ratio,7.7095) + 0.111;
            }

            var distanceRound = Math.Round(distance, 1);
            //Debug.WriteLine($"{rssi}  {distanceRound:F2}");
            return distanceRound;
        }
        
        
        /// <summary>
        /// Вычислить расстояние по силе сигнала.
        /// Округляет вычисления до 1 знака после запятой
        /// </summary>
        /// <param name="measuredPower"></param>
        /// <param name="rssi"></param>
        /// <returns></returns>
        public static Result<double> CalculateDistance2(int measuredPower, int rssi)
        {
            double distance;
            if (rssi == 0) {
                return Result.Failure<double>("rssi == 0"); // if we cannot determine distance, return -1.
            }
            double ratio = rssi*1.0/measuredPower;
            if (ratio < 1.0) 
            {
                distance= Math.Pow(ratio,10);
            }
            else
            {
                distance =  (0.89976)*Math.Pow(ratio,7.7095) + 0.111;
            }

            var distanceRound = Math.Round(distance, 1);
            //Debug.WriteLine($"{rssi}  {distanceRound:F2}");
            return distanceRound;
        }
    }
}