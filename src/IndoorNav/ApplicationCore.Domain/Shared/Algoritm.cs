using System;

namespace ApplicationCore.Domain.Shared
{
    
    public static class Algoritm
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
            else {
                double distance =  (0.89976)*Math.Pow(ratio,7.7095) + 0.111;
                return distance;
            }
        }
        
        
        public static double CalculateDistance(int measuredPower, int rssi) 
        {
            if (rssi == 0) {
                return -1.0; // if we cannot determine distance, return -1.
            }
            double ratio = rssi*1.0/measuredPower;
            if (ratio < 1.0) {
                return Math.Pow(ratio,10);
            }
            else {
                double distance =  (0.89976)*Math.Pow(ratio,7.7095) + 0.111;
                return distance;
            }
        }
    }
}