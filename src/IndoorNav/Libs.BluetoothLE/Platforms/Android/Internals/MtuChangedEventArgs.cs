using Android.Bluetooth;

namespace Libs.BluetoothLE.Platforms.Android.Internals
{
    public class MtuChangedEventArgs : GattEventArgs
    {
        public int Mtu { get; }


        public MtuChangedEventArgs(int mtu, BluetoothGatt gatt, GattStatus status) : base(gatt, status)
        {
            this.Mtu = mtu;
        }
    }
}