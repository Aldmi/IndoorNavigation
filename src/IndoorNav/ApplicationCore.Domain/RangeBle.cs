using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Domain
{
    public class RangeBle
    {
        private RangeBle(int rssi, double value)
        {
            Rssi = rssi;
            Value = value;
        }

        public int Rssi { get; }
        public double Value { get; }



        public static RangeBle Create(int rssi, int txPower, Func<int, int, double> rssi2RangeConverter)
        {
            return new RangeBle(rssi, rssi2RangeConverter(txPower, rssi));
        }


        public static double CalcAverageValue(IEnumerable<RangeBle> ranges)=>
            ranges.Average(r => r.Value);


        public override string ToString() => $"{Rssi}Db -> {Value:F2}м";
    }
}