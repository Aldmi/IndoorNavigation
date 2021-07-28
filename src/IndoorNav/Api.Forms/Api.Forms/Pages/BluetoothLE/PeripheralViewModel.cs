using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using Api.Forms.Infrastructure;
using Libs.BluetoothLE;
using Prism.Navigation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Shiny;
using Shiny.BluetoothLE;

namespace Api.Forms.Pages.BluetoothLE
{
    public class PeripheralViewModel : ViewModel
    {
        private readonly IDialogs _dialogs;
        private IPeripheral _peripheral;


        public PeripheralViewModel(IDialogs dialogs)
        {
            _dialogs = dialogs;

            SelectCharacteristic = ReactiveCommand.Create<GattCharacteristicViewModel>(x => x.Select());

            ConnectionToggle = ReactiveCommand.Create(() =>
            {
                // don't cleanup connection - force user to d/c
                if (_peripheral.Status == ConnectionState.Connecting || _peripheral.Status == ConnectionState.Connecting)
                {
                    _peripheral.CancelConnection();
                }
                else
                {
                    _peripheral.Connect();
                }
            });

            PairToDevice = ReactiveCommand.CreateFromTask(async () =>
            {
                var pair = _peripheral as ICanPairPeripherals;

                if (pair == null)
                {
                    await dialogs.Alert("Pairing is not supported on this platform");
                }
                else if (pair.PairingStatus == PairingState.Paired)
                {
                    await dialogs.Snackbar("Peripheral is already paired");
                }
                else
                {
                    var result = await pair.PairingRequest(); //.Timeout(TimeSpan.FromSeconds(10));
                    await dialogs.Snackbar(result ? "Peripheral Paired Successfully" : "Peripheral Pairing Failed");
                    this.RaisePropertyChanged(nameof(PairingText));
                }
            });

            RequestMtu = ReactiveCommand.CreateFromTask(
                async x =>
                {
                    var mtu = _peripheral as ICanRequestMtu;
                    if (mtu == null)
                    {
                        await dialogs.Alert("MTU requests are not supported on this platform");
                    }
                    else
                    {
                        var result = await dialogs.Input("MTU Request", "Range 20-512");
                        if (!result.IsEmpty())
                        {
                            var actual = await mtu.RequestMtu(Int32.Parse(result));
                            await dialogs.Snackbar("MTU Changed to " + actual);
                        }
                    }
                },
                this.WhenAny(
                    x => x.ConnectText,
                    x => x.GetValue().Equals("Disconnect")
                )
            );
        }


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _peripheral = parameters.GetValue<IPeripheral>("Peripheral");
            Name = _peripheral.Name;
            Uuid = _peripheral.Uuid;

            IsMtuVisible = _peripheral.IsMtuRequestsAvailable();
            IsPairingVisible = _peripheral.IsPairingRequestsAvailable();

            PairingText = _peripheral.TryGetPairingStatus() == PairingState.Paired
                ? "Peripheral Paired"
                : "Pair Peripheral";

            //this.peripheral
            //    .WhenConnected()
            //    .Select(x => x.ReadRssiContinuously(TimeSpan.FromSeconds(3)))
            //    .Switch()
            //    .SubOnMainThread(x => this.Rssi = x)
            //    .DisposedBy(this.DeactivateWith);

            _peripheral
                .WhenStatusChanged()
                .Select(x => x switch
                {
                    ConnectionState.Connecting => "Cancel Connection",
                    ConnectionState.Connected => "Disconnect",
                    ConnectionState.Disconnected => "Connect",
                    ConnectionState.Disconnecting => "Disconnecting..."
                })
                .SubOnMainThread(x =>
                {
                    ConnectText = x;
                })
                .DisposedBy(DeactivateWith);

            _peripheral
                .WhenConnected()
                .Do(x => GattCharacteristics.Clear())
                .SelectMany(x => x.GetAllCharacteristics())
                .Select(x => x.Select(y => new GattCharacteristicViewModel(y, _dialogs)))
                .SubOnMainThread(
                    list =>
                    {
                        var chars = list
                            .GroupBy(x => x.ServiceUuid)
                            .Select(x =>
                            {
                                var group = new Group<GattCharacteristicViewModel>(x.Key, x.Key);
                                group.AddRange(x.ToList());
                                return group;
                            })
                            .ToList();

                        GattCharacteristics.AddRange(chars);
                    },
                    ex => _dialogs.Snackbar(ex.ToString())
                )
                .DisposedBy(DeactivateWith);
        }


        public ICommand ConnectionToggle { get; }
        public ICommand PairToDevice { get; }
        public ICommand RequestMtu { get; }
        public ICommand SelectCharacteristic { get; }

        [Reactive] public string Name { get; private set; }
        [Reactive] public string Uuid { get; private set; }
        [Reactive] public string PairingText { get; private set; }
        public ObservableList<Group<GattCharacteristicViewModel>> GattCharacteristics { get; } = new ObservableList<Group<GattCharacteristicViewModel>>();

        [Reactive] public string ConnectText { get; private set; } = "Connect";
        [Reactive] public int Rssi { get; private set; }

        [Reactive] public bool IsMtuVisible { get; private set; }
        [Reactive] public bool IsPairingVisible { get; private set; }
    }
}
