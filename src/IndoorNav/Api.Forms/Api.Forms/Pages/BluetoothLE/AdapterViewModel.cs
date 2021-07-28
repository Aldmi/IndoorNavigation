using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using Api.Forms.Infrastructure;
using Libs.BluetoothLE;
using Microsoft.Extensions.Logging;
using Prism.Navigation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Shiny;

namespace Api.Forms.Pages.BluetoothLE
{
    public class AdapterViewModel : ViewModel
    {
        private readonly ILogger<AdapterViewModel> _logger;
        private IDisposable? _scanSub;


        public AdapterViewModel(INavigationService navigator,
                                IDialogs dialogs,
                                IBleManager bleManager,
                                ILogger<AdapterViewModel> logger)
        {
            _logger = logger;
            IsScanning = bleManager?.IsScanning ?? false;
            CanControlAdapterState = bleManager?.CanControlAdapterState() ?? false;

            this.WhenAnyValue(x => x.SelectedPeripheral)
                .Skip(1)
                .Where(x => x != null)
                .Subscribe(async x =>
                {
                    SelectedPeripheral = null;
                    StopScan();
                    await navigator.Navigate("Peripheral", ("Peripheral", x.Peripheral));
                });

            ToggleAdapterState = ReactiveCommand.CreateFromTask(
                async () =>
                {
                    if (bleManager == null)
                    {
                        await dialogs.Alert("Platform Not Supported");
                    }
                    else
                    {
                        _logger.LogError("DEBUG ERROR");//DEBUG
                        if (bleManager.Status == AccessState.Available)
                        {
                            await bleManager.TrySetAdapterState(false);
                            await dialogs.Snackbar("Bluetooth Adapter Disabled");
                        }
                        else
                        {
                            await bleManager.TrySetAdapterState(true);
                            await dialogs.Snackbar("Bluetooth Adapter Enabled");
                        }
                    }
                }
            );

            ScanToggle = ReactiveCommand.CreateFromTask(
                async () =>
                {
                    if (bleManager == null)
                    {
                        await dialogs.Alert("Platform Not Supported");
                        return;
                    }
                    if (IsScanning)
                    {
                        StopScan();
                    }
                    else
                    {
                        Peripherals.Clear();
                        IsScanning = true;

                        var scanConf = new ScanConfig
                        {
                            ScanType = BleScanType.Balanced
                        };
                        
                        //All scan for DEBUG output-------------------------
                        // _scanSub = bleManager
                        //     .Scan(scanConf)
                        //     .Where(x => IsBeacon(x))
                        //     .Buffer(TimeSpan.FromSeconds(1))
                        //     .Where(x => x?.Any() ?? false)
                        //     .SubOnMainThread(
                        //         results =>
                        //         {
                        //             foreach (var result in results)
                        //             {
                        //                 var md = result.AdvertisementData.ManufacturerData?.Data;
                        //                 var mdStr = md == null ? " " : ByteArrayToString(md);
                        //                 
                        //                 Debug.WriteLine($"{DateTime.Now.TimeOfDay:T} Rssi= '{result.Rssi}'" +
                        //                                 $"\t Peripheral.Name='{result.Peripheral.Name}'  Peripheral.Status='{result.Peripheral.Status}'  Peripheral.Uuid='{result.Peripheral.Uuid}'" +
                        //                                 $"\t AdvertisementData.TxPower='{result.AdvertisementData.TxPower}'  AdvertisementData.LocalName='{result.AdvertisementData.LocalName}'  " +
                        //                                 $" AdvertisementData.ManufacturerData= {mdStr} ");
                        //                 
                        //                 Debug.WriteLine("-----------------------------------------------------");
                        //             }
                        //           
                        //         },
                        //         ex => dialogs.Alert(ex.ToString(), "ERROR")
                        //     );
                        
                        // //Sphere Scan (без фильтра по UUID)----------------------------------------------
                        // _scanSub = bleManager
                        //     .Scan(scanConf)
                        //     .Where(x => IsBeacon(x))
                        //     .Select(x => x.AdvertisementData.ManufacturerData.Data.Parse(x.Rssi))
                        //     .SubOnMainThread(
                        //         beacon =>
                        //         {
                        //             Debug.WriteLine($"{DateTime.Now.TimeOfDay:T}  Uuid= '{beacon.Uuid}  Rssi= '{beacon.Rssi}' '{beacon.Major}-{beacon.Minor}''");
                        //             Debug.WriteLine("-----------------------------------------------------");
                        //         },
                        //         ex => dialogs.Alert(ex.ToString(), "ERROR")
                        //     );
                        
                        _scanSub = bleManager
                            .Scan(scanConf)
                            .Where(x => IsBeacon(x))   //Только маяки
                            .Buffer(TimeSpan.FromSeconds(0.1))
                            .Where(x => x?.Any() ?? false)
                            .SubOnMainThread(
                                results =>
                                {
                                    var list = new List<PeripheralItemViewModel>();
                                    foreach (var result in results)
                                    {
                                        var peripheral = Peripherals.FirstOrDefault(x => x.Equals(result.Peripheral));
                                        if (peripheral == null)
                                            peripheral = list.FirstOrDefault(x => x.Equals(result.Peripheral));
                        
                                        if (peripheral != null)
                                        {
                                            peripheral.Update(result);
                                        }
                                        else
                                        {
                                            peripheral = new PeripheralItemViewModel(result.Peripheral);
                                            peripheral.Update(result);
                                            list.Add(peripheral);
                                        }
                                    }
                                    if (list.Any())
                                    {
                                        // XF is not able to deal with an observablelist/addrange properly
                                        foreach (var item in list)
                                            Peripherals.Add(item);
                                    }
                                },
                                ex => dialogs.Alert(ex.ToString(), "ERROR")
                            );
                    }
                }
            );
        }

        public static bool IsBeacon(ScanResult result)
        {
            var md = result.AdvertisementData?.ManufacturerData;

            if (md == null || md.Data == null || md.Data.Length != 23)
                return false;

            if (md.CompanyId != 76) //
                return false;
            
            return IsBeaconPacket(md.Data);
        }
        
        public static bool IsBeaconPacket(byte[] data, bool skipManufacturerByte = true)
        {
            if (data == null)
                return false;

            if (data.Length != 23)
                return false;

            // apple manufacturerID - https://www.bluetooth.com/specifications/assigned-numbers/company-Identifiers
            if (!skipManufacturerByte && data[0] != 76)
                return false;

            return true;
        }
        
        //
        // /// <summary>
        // /// This should not be called by iOS as txpower is not available in the advertisement packet
        // /// </summary>
        // /// <returns>The beacon.</returns>
        // /// <param name="data">Data.</param>
        // /// <param name="rssi">Rssi.</param>
        // public static Sphere Parse(byte[] data, int rssi)
        // {
        //     if (BitConverter.IsLittleEndian)
        //     {
        //         var uuidString = BitConverter.ToString(data, 2, 16).Replace("-", String.Empty);
        //         var uuid = new Guid(uuidString);
        //         var major = BitConverter.ToUInt16(data.Skip(18).Take(2).Reverse().ToArray(), 0);
        //         var minor = BitConverter.ToUInt16(data.Skip(20).Take(2).Reverse().ToArray(), 0);
        //         var txpower = data[22];
        //         var accuracy = CalculateAccuracy(txpower, rssi);
        //         var proximity = CalculateProximity(txpower, rssi);
        //
        //         return new Sphere(uuid, major, minor, proximity, rssi, accuracy);
        //     }
        //     throw new ArgumentException("TODO");
        // }
        //
        // public static double CalculateAccuracy(int txPower, double rssi)
        // {
        //     var accuracy = -1.0;
        //     if (rssi > 0)
        //     {
        //         var ratio = rssi * 1.0 / txPower;
        //         if (ratio < 1.0)
        //         {
        //             accuracy = Math.Pow(ratio, 10);
        //         }
        //         else
        //         {
        //             accuracy = 0.89976 * Math.Pow(ratio, 7.7095) + 0.111;
        //         }
        //     }
        //     return accuracy;
        // }
        //
        // // 6 meters+ = far
        // // 2 meters+ = near
        // // 0.5 meters += immediate
        //
        // public static Proximity CalculateProximity(int txpower, double rssi)
        // {
        //     var distance = Math.Pow(10d, (txpower * -1 - rssi) / 20);
        //     if (distance >= 6E-6d)
        //         return Proximity.Far;
        //
        //     if (distance > 0.5E-6d)
        //         return Proximity.Near;
        //
        //     return Proximity.Immediate;
        // }
        //
        //
        // public static string ByteArrayToString(byte[] ba)
        // {
        //     return BitConverter.ToString(ba).Replace("-","");
        // }        //
        // /// <summary>
        // /// This should not be called by iOS as txpower is not available in the advertisement packet
        // /// </summary>
        // /// <returns>The beacon.</returns>
        // /// <param name="data">Data.</param>
        // /// <param name="rssi">Rssi.</param>
        // public static Sphere Parse(byte[] data, int rssi)
        // {
        //     if (BitConverter.IsLittleEndian)
        //     {
        //         var uuidString = BitConverter.ToString(data, 2, 16).Replace("-", String.Empty);
        //         var uuid = new Guid(uuidString);
        //         var major = BitConverter.ToUInt16(data.Skip(18).Take(2).Reverse().ToArray(), 0);
        //         var minor = BitConverter.ToUInt16(data.Skip(20).Take(2).Reverse().ToArray(), 0);
        //         var txpower = data[22];
        //         var accuracy = CalculateAccuracy(txpower, rssi);
        //         var proximity = CalculateProximity(txpower, rssi);
        //
        //         return new Sphere(uuid, major, minor, proximity, rssi, accuracy);
        //     }
        //     throw new ArgumentException("TODO");
        // }
        //
        // public static double CalculateAccuracy(int txPower, double rssi)
        // {
        //     var accuracy = -1.0;
        //     if (rssi > 0)
        //     {
        //         var ratio = rssi * 1.0 / txPower;
        //         if (ratio < 1.0)
        //         {
        //             accuracy = Math.Pow(ratio, 10);
        //         }
        //         else
        //         {
        //             accuracy = 0.89976 * Math.Pow(ratio, 7.7095) + 0.111;
        //         }
        //     }
        //     return accuracy;
        // }
        //
        // // 6 meters+ = far
        // // 2 meters+ = near
        // // 0.5 meters += immediate
        //
        // public static Proximity CalculateProximity(int txpower, double rssi)
        // {
        //     var distance = Math.Pow(10d, (txpower * -1 - rssi) / 20);
        //     if (distance >= 6E-6d)
        //         return Proximity.Far;
        //
        //     if (distance > 0.5E-6d)
        //         return Proximity.Near;
        //
        //     return Proximity.Immediate;
        // }
        //
        //
        // public static string ByteArrayToString(byte[] ba)
        // {
        //     return BitConverter.ToString(ba).Replace("-","");
        // }


        
        public ICommand ScanToggle { get; }
        public ICommand ToggleAdapterState { get; }
        public bool CanControlAdapterState { get; }
        public ObservableCollection<PeripheralItemViewModel> Peripherals { get; } = new ObservableCollection<PeripheralItemViewModel>();
        [Reactive] public PeripheralItemViewModel? SelectedPeripheral { get; set; }
        [Reactive] public bool IsScanning { get; private set; }


        private void StopScan()
        {
            _scanSub?.Dispose();
            _scanSub = null;
            IsScanning = false;
        }
    }
}