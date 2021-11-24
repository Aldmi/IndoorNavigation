using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using Api.Forms.Infrastructure;
using Libs.BluetoothLE;
using ReactiveUI.Fody.Helpers;
using Shiny;
using Xamarin.Forms;

namespace Api.Forms.Pages.BluetoothLE
{
    public class GattCharacteristicViewModel : ViewModel
    {
        private readonly IDialogs _dialogs;
        private IDisposable _watcher;


        public GattCharacteristicViewModel(IGattCharacteristic characteristic, IDialogs dialogs)
        {
            Characteristic = characteristic;
            _dialogs = dialogs;
        }


        public IGattCharacteristic Characteristic { get; }


        [Reactive] public string Value { get; private set; }
        [Reactive] public bool IsNotifying { get; private set; }
        [Reactive] public bool IsValueAvailable { get; private set; }
        [Reactive] public DateTime LastValue { get; private set; }
        public string Uuid => Characteristic.Uuid;
        public string ServiceUuid => Characteristic.Service.Uuid;
        public string Properties => Characteristic.Properties.ToString();


        public async void Select()
        {
            var cfg = new Dictionary<string, Action>();
            if (Characteristic.CanWriteWithResponse())
                cfg.Add("Write With Response", () => DoWrite(true));

            if (Characteristic.CanWriteWithoutResponse())
                cfg.Add("Write Without Response", () => DoWrite(false));

            if (Characteristic.CanWrite())
                cfg.Add("Send Test BLOB", SendBlob);

            if (Characteristic.CanRead())
                cfg.Add("Read", DoRead);

            if (Characteristic.CanNotify())
            {
                var txt = Characteristic.IsNotifying ? "StopScan Notifying" : "Notify";
                cfg.Add(txt, ToggleNotify);
            }
            if (cfg.Any())
                await _dialogs.ActionSheet($"{Uuid}", cfg);
        }


        private async void SendBlob()
        {
            var cts = new CancellationTokenSource();
            var bytes = Encoding.UTF8.GetBytes(RandomString(5000));
            //var dlg = this.dialogs.Loading("Sending Blob", () => cts.Cancel(), "Cancel");
            var sw = new Stopwatch();
            sw.Start();

            var sub = Characteristic
                .WriteBlob(new MemoryStream(bytes))
                .Subscribe(
                    //s => dlg.Title = $"Sending Blob - Sent {s.Position} of {s.TotalLength} bytes",
                    ex =>
                    {
                        //dlg.Dispose();
                        _dialogs.Snackbar("Failed writing blob - " + ex);
                        sw.Stop();
                    },
                    () =>
                    {
                        //dlg.Dispose();
                        sw.Stop();
                        _dialogs.Snackbar("BLOB write took " + sw.Elapsed);
                    }
                );

            cts.Token.Register(() => sub.Dispose());
        }


        private async void DoWrite(bool withResponse)
        {
            try
            {
                var utf8 = await _dialogs.Confirm("Confirm", "Write Value from UTF8 or HEX?", "UTF8", "HEX");
                var result = await _dialogs.Input(Uuid, "Please enter a write Value");

                if (!result.IsEmpty())
                {
                    var bytes = utf8 ? Encoding.UTF8.GetBytes(result) : result.FromHex();
                    var msg = withResponse ? "Write Complete" : "Write Without Response Complete";
                    Characteristic
                        .Write(bytes, withResponse)
                        .Timeout(TimeSpan.FromSeconds(2))
                        .Subscribe(
                            x => _dialogs.Snackbar(msg),
                            ex => _dialogs.Alert(ex.ToString())
                        );
                }
            }
            catch (Exception ex)
            {
                await _dialogs.Alert(ex.ToString(), "ERROR");
            }
        }


        private async void ToggleNotify()
        {
            if (Characteristic.IsNotifying)
            {
                _watcher?.Dispose();
                IsNotifying = false;
            }
            else
            {
                IsNotifying = true;
                var utf8 = await _dialogs.Confirm(
                    "Display Value as UTF8 or HEX?",
                    "Confirm",
                    "UTF8",
                    "HEX"
                );
                _watcher = Characteristic
                    .Notify()
                    .Where(x => x.Type == GattCharacteristicResultType.Notification)
                    .SubOnMainThread(
                        x => SetReadValue(x, utf8),
                        ex => _dialogs.Alert(ex.ToString())
                    );
            }
        }


        private async void DoRead()
        {
            var utf8 = await _dialogs.Confirm(
                "Confirm",
                "Display Value as UTF8 or HEX?",
                "UTF8",
                "HEX"
            );
            Characteristic
                .Read()
                .Timeout(TimeSpan.FromSeconds(2))
                .Subscribe(
                    x => SetReadValue(x, utf8),
                    ex => _dialogs.Alert(ex.ToString())
                );
        }


        private void SetReadValue(GattCharacteristicResult result, bool fromUtf8) => Device.BeginInvokeOnMainThread(() =>
        {
            IsValueAvailable = true;
            LastValue = DateTime.Now;

            if (result.Data == null)
                Value = "EMPTY";

            else
                Value = fromUtf8
                    ? Encoding.UTF8.GetString(result.Data, 0, result.Data.Length)
                    : BitConverter.ToString(result.Data);
        });


        private static readonly Random Random = new Random();

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}
