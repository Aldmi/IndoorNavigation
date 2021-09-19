using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;
using Api.Forms.Infrastructure;
using Libs.Beacons.Models;
using Prism.Navigation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using UseCase.Trilateration.Managed;

namespace Api.Forms.Pages.Beacons
{
    public class ManagedBeaconViewModel : ViewModel
    {
        private readonly INavigationService _navigator;
        private readonly IDialogs _dialogs;
        private readonly ManagedScan _scanner;
        private BeaconRegion? _region;
        
        
        public ManagedBeaconViewModel(ManagedScan beaconManager, INavigationService navigator, IDialogs dialogs)
        {
            _scanner = beaconManager;
            _scanner.ClearTime= TimeSpan.FromSeconds(2); //Если нет данных от маяка в течении 2сек, то убираем его из списка (сама проверка выполняется раз в 5 сек)
            _navigator = navigator;
            _dialogs = dialogs;
            
            SetRegion = navigator.NavigateCommand(
                "CreateBeacon",
                
                p => p
                    .Set(nameof(BeaconRegion), _region)
            );
            // this.WhenAny(
            //     x => ScanText,
            //     (idValue) =>
            //     {
            //         return (!_scanner.IsScanning);
            //     })
            
            
            this.WhenAnyValue(x => x.Region)
                .Select(region => region != null)
                .ToPropertyEx(this, x => x.IsRegionSet);
            
            this.WhenAnyValue(x => x.Region.Major)
                .Select(major => major != null)
                .ToPropertyEx(this, x => x.IsMajorSet);
            
            this.WhenAnyValue(x => x.Region.Minor)
                .Select(minor => minor != null)
                .ToPropertyEx(this, x => x.IsMinorSet);

            this.WhenAnyValue(x => x.ExpectedRange)
                .Subscribe(er => _scanner.ExpectedRange4Analitic = er);
            
            
            ClearRegion = ReactiveCommand.Create(() =>
            {
                Region = null;
                Statistics.Clear();//TODO: ??? прокинуть команду в MyManagedScan по очищению списка.
            });
            
            ScanToggle = ReactiveCommand.Create(() =>
            {
                if (!_scanner.IsScanning)
                    StartScan();
                else
                    StopScan();
            },this.WhenAny(
                x => x.IsRegionSet,
                _ => IsRegionSet));
            
            IncExpectedRange = ReactiveCommand.Create(() =>
            {
                if (ExpectedRange < 10) ExpectedRange++;
            });
            
            DecExpectedRange = ReactiveCommand.Create(() =>
            {
                if (ExpectedRange > 0) ExpectedRange--;
            });
        }

        
        public ObservableCollection<BeaconDistanceStatisticDto> Statistics => _scanner.Statistic;
        
        public ICommand SetRegion { get; }
        public ICommand ClearRegion { get; }
        public ICommand ScanToggle { get; }
        public ICommand IncExpectedRange { get; }
        public ICommand DecExpectedRange { get; }
        
        [Reactive] public BeaconRegion? Region { get; private set; }
        [Reactive] public string ScanText { get; private set; } = "Scan";
        [Reactive] public int ExpectedRange  { get; private set; }
        
        public bool IsRegionSet { [ObservableAsProperty] get; }
        public bool IsMajorSet { [ObservableAsProperty] get;  }
        public bool IsMinorSet { [ObservableAsProperty] get; }

        
        
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
             _region = parameters.GetValue<BeaconRegion>(nameof(BeaconRegion));
             Region = _region;
             StartScan();
        }



        public void StartScan()
        {
            if(_scanner.IsScanning)
                return;
             
            ScanText = "Stop Scan";
            if (IsRegionSet) //TODO: пока нельзя запускать сканер без региона.
            {
                try
                {
                    _scanner.Start(_region, RxApp.MainThreadScheduler);
                    IsBusy = true;
                }
                catch (Exception e)
                {
                    _dialogs.Alert($"{e.Message}","Ошибка сканирования" );
                }
            }
        }
        
        
        public void StopScan()
        {
            ScanText = "Scan";
            _scanner.Stop();
        }


        public override void OnAppearing()
        {
            base.OnAppearing();
        }
        
        public override void OnDisappearing()
        {
            //IsBusy = false;
            //_scanner.Stop();
        }
    }
}
