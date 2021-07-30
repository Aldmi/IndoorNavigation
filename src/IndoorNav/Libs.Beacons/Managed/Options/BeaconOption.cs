using System;
using Libs.Beacons.Managed.Flows.TrilaterationFlow;
using Libs.Beacons.Models;

namespace Libs.Beacons.Managed.Options
{
    /// <summary>
    /// Настройки маяка.
    /// </summary>
    public class BeaconOption
    {

        public BeaconOption(Guid uuid,ushort major, ushort minor, int n, int txPower, Point ancore)
        {
            Uuid = uuid;
            Major = major;
            Minor = minor;
            N = n;
            TxPower = txPower;
            Ancore = ancore;
        }
        
        
        public Guid Uuid { get; }
        public ushort Minor { get; }
        public ushort Major { get; }
        
        
        /// <summary>
        /// Коэффициент искажения сигнала
        /// </summary>
        public int N { get; }

        /// <summary>
        /// Мощность на расстоянии 1 метр
        /// </summary>
        public int TxPower { get; }

        /// <summary>
        /// Положение маяка на координатной сетке
        /// </summary>
        public Point Ancore { get; }



        public bool Equal2Beacon(Beacon beacon) =>
            beacon.Uuid == Uuid && beacon.Minor == Minor && beacon.Major == Major;
        

    }
}