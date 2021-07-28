using Android.Bluetooth;

namespace Libs.BluetoothLE.Platforms.Android.Internals
{
    public class ConnectionStateEventArgs : GattEventArgs
    {
        public ProfileState NewState { get; }


        public ConnectionStateEventArgs(BluetoothGatt gatt, GattStatus status, ProfileState newState) : base(gatt, status)
        {
            this.NewState = newState;
        }
    }
}