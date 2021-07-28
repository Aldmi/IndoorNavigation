using Shiny.BluetoothLE;

namespace Libs.BluetoothLE
{
    public interface IAdvertisementData
    {
        string? LocalName { get; }
        bool? IsConnectable { get; }
        AdvertisementServiceData[]? ServiceData { get; }
        ManufacturerData? ManufacturerData { get; }
        string[]? ServiceUuids { get; }
        int TxPower { get; }
    }
}
