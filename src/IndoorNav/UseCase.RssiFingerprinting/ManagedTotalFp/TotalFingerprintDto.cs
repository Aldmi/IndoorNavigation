using ApplicationCore.Shared.Models;
using Shiny;

namespace UseCase.RssiFingerprinting.ManagedTotalFp
{
    public class TotalFingerprintDto : NotifyPropertyChanged
    {
        public TotalFingerprintDto(Point roomCoordinate)
        {
            RoomCoordinateX = roomCoordinate.X;
            RoomCoordinateY = roomCoordinate.Y;
        }

       
        private double _roomCoordinateX;
        public double RoomCoordinateX
        {
            get => _roomCoordinateX;
            internal set => Set(ref _roomCoordinateX, value);
        }
        
        
        private double _roomCoordinateY;
        public double RoomCoordinateY
        {
            get => _roomCoordinateY;
            internal set => Set(ref _roomCoordinateY, value);
        }
    }
}