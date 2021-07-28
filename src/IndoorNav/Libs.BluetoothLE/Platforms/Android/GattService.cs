using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using Android.Bluetooth;
using Libs.BluetoothLE.Platforms.Android.Internals;
using Shiny;
using Shiny.BluetoothLE;
using Observable = System.Reactive.Linq.Observable;


namespace Libs.BluetoothLE.Platforms.Android
{
    public class GattService : AbstractGattService
    {
        readonly PeripheralContext context;
        readonly BluetoothGattService native;


        public GattService(
            IPeripheral peripheral,
            PeripheralContext context,
            BluetoothGattService native
        ): base(
            peripheral,
            native.Uuid.ToString(),
            native.Type == GattServiceType.Primary
        )
        {
            this.context = context;
            this.native = native;
        }


        public override IObservable<IList<IGattCharacteristic>> GetCharacteristics() => Observable.Return(
            this.native
                .Characteristics
                .Select(native => new GattCharacteristic(this, this.context, native))
                .Cast<IGattCharacteristic>()
                .ToList()
        );


        public override IObservable<IGattCharacteristic?> GetKnownCharacteristic(string characteristicUuid, bool throwIfNotFound = false)
            => Observable.Create<IGattCharacteristic?>(ob =>
            {
                var uuid = Utils.ToUuidType(characteristicUuid);
                var cs = this.native.GetCharacteristic(uuid);

                if (cs == null)
                {
                    ob.Respond(null);
                }
                else
                {
                    var characteristic = new GattCharacteristic(this, this.context, cs);
                    ob.Respond(characteristic);
                }
                return Disposable.Empty;
            })
            .Assert(this.Uuid, characteristicUuid, throwIfNotFound);


        public override bool Equals(object obj)
        {
            var other = obj as GattService;
            if (other == null)
                return false;

            if (!Object.ReferenceEquals(this, other))
                return false;

            return true;
        }


        public override int GetHashCode() => this.native.GetHashCode();
        public override string ToString() => $"Peripheral {this.Uuid}";
    }
}