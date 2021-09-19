using ApplicationCore.Shared;
using ApplicationCore.Shared.Models;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.Options
{
    /// <summary>
    /// Настройки маяка.
    /// </summary>
    public class BeaconOption 
    {
        public BeaconOption(BeaconId beaconId, int n, int txPower, Point ancore)
        {
            BeaconId = beaconId;
            N = n;
            TxPower = txPower;
            Ancore = ancore;
        }


        public BeaconId BeaconId { get; }

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

        
        public bool EqualById(BeaconId beaconid) => BeaconId == beaconid;
    }
}