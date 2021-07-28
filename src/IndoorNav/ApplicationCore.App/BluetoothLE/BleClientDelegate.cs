﻿// using System;
// using System.Threading.Tasks;
// using ApplicationCore.App.Infrastructure;
// using ApplicationCore.App.Models;
// using Shiny;
// using Shiny.BluetoothLE;
//
// namespace ApplicationCore.App.BluetoothLE
// {
//     public class BleClientDelegate : BleDelegate, IShinyStartupTask
//     {
//         readonly CoreDelegateServices services;
//         public BleClientDelegate(CoreDelegateServices services) => this.services = services;
//
//
//         public override async Task OnAdapterStateChanged(AccessState state)
//         {
//             if (state == AccessState.Disabled)
//                 await services.Notifications.Send(GetType(), true, "BLE State", "Turn on Bluetooth already");
//         }
//
//
//         //public override Task OnConnected(IPeripheral peripheral) =>
//         //    this.services.Connection.InsertAsync(new BleEvent
//         //    {
//         //        Timestamp = DateTime.Now
//         //    });
//
//         public override async Task OnConnected(IPeripheral peripheral)
//         {
//             await services.Connection.InsertAsync(new BleEvent
//             {
//                 Description = $"Peripheral '{peripheral.Name}' Connected",
//                 Timestamp = DateTime.Now
//             });
//             await services.Notifications.Send(
//                 GetType(),
//                 true,
//                 "BluetoothLE Device Connected",
//                 $"{peripheral.Name} has connected"
//             );
//         }
//
//
//         //public override Task OnScanResult(ScanResult result)
//         //{
//         //    // we only want this to run in the background
//         //    return base.OnScanResult(result);
//         //}
//
//
//         public void Start()
//             => services.Notifications.Register(GetType(), false, "BluetoothLE");
//     }
// }
