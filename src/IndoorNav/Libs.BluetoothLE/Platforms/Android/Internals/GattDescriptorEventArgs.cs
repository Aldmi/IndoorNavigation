using Android.Bluetooth;

namespace Libs.BluetoothLE.Platforms.Android.Internals
{
    public class GattDescriptorEventArgs : GattEventArgs
    {
        public BluetoothGattDescriptor Descriptor { get; }


        public GattDescriptorEventArgs(BluetoothGatt gatt, BluetoothGattDescriptor descriptor, GattStatus status) : base(gatt, status)
        {
            this.Descriptor = descriptor;
        }
    }
}