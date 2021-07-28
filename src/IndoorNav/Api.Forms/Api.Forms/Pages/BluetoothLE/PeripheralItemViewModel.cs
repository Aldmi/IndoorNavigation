using Api.Forms.Infrastructure;
using Libs.BluetoothLE;
using ReactiveUI.Fody.Helpers;

namespace Api.Forms.Pages.BluetoothLE
{
    public class PeripheralItemViewModel : ViewModel
    {
        public PeripheralItemViewModel(IPeripheral peripheral)
            => Peripheral = peripheral;


        public override bool Equals(object obj)
            => Peripheral.Equals(obj);

        public IPeripheral Peripheral { get; }
        public string Uuid => Peripheral.Uuid;

        [Reactive] public string Name { get; private set; }
        [Reactive] public bool IsConnected { get; private set; }
        [Reactive] public int Rssi { get; private set; }
        [Reactive] public string Connectable { get; private set; }
        [Reactive] public int ServiceCount { get; private set; }
        //[Reactive] public string ManufacturerData { get; private set; }
        [Reactive] public string LocalName { get; private set; }
        [Reactive] public int TxPower { get; private set; }


        public void Update(ScanResult result)
        {
            // using (SuppressChangeNotifications())
            {
                Name = Peripheral.Name;
                Rssi = result.Rssi;

                var ad = result.AdvertisementData;
                ServiceCount = ad.ServiceUuids?.Length ?? 0;
                Connectable = ad?.IsConnectable?.ToString() ?? "Unknown";
                LocalName = ad.LocalName;
                TxPower = ad.TxPower;
                //this.ManufacturerData = ad.ManufacturerData == null
                //    ? null
                //    : BitConverter.ToString(ad.ManufacturerData);
            }
        }
    }
}