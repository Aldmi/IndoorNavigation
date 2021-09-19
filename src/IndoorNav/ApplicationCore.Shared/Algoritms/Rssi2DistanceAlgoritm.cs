using System;

namespace ApplicationCore.Shared.Algoritms
{
    public static class Rssi2DistanceAlgoritm
    {
        public static double CalculateDistance(int measuredPower, double rssi) 
        {
            if (rssi == 0) {
                return -1.0;
            }
            double ratio = rssi*1.0/measuredPower;
            if (ratio < 1.0) {
                return Math.Pow(ratio,10);
            }

            double distance =  (0.89976)*Math.Pow(ratio,7.7095) + 0.111;
            return distance;
        }
        
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
    }
}