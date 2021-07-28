using System;
using System.Reactive;

namespace Libs.BluetoothLE
{
    public interface IGattReliableWriteTransaction : IDisposable
    {
        TransactionState Status { get; }
        IObservable<GattCharacteristicResult> Write(IGattCharacteristic characteristic, byte[] value);
        IObservable<Unit> Commit();
        void Abort();
    }
}