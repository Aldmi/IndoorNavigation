using System;

namespace Libs.BluetoothLE
{
    public class GattReliableWriteTransactionException : Exception
    {
        public GattReliableWriteTransactionException(string msg) : base(msg) { }
    }
}
