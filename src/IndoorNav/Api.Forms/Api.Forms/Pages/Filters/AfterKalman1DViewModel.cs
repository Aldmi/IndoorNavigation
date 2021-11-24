using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Api.Forms.Infrastructure;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using UseCase.RssiFingerprinting.ManagedKalmanStatistic;
using UseCase.Trilateration.Managed;

namespace Api.Forms.Pages.Filters
{
    public class AfterKalman1DViewModel : ViewModel
    {
        private readonly ManagedKalman1D _scanner;
        private readonly IDialogs _dialogs;

        
        public AfterKalman1DViewModel(ManagedKalman1D scanner, IDialogs dialogs)
        {
            _scanner = scanner;
            _dialogs = dialogs;
            
            ScanToggle = ReactiveCommand.Create(() =>
            {
                if (!_scanner.IsScanning)
                    StartScan();
                else
                    StopScan();
            });
            
            
            this.WhenAnyValue(x => x.ExpectedRange)
                .Subscribe(er => _scanner.ExpectedRange = er);
            
            IncExpectedRange = ReactiveCommand.Create(() =>
            {
                if (ExpectedRange < 10) ExpectedRange++;
            });
            
            DecExpectedRange = ReactiveCommand.Create(() =>
            {
                if (ExpectedRange > 0) ExpectedRange--;
            });
        }
        
        
        public ObservableCollection<AfterKalman1DStatisticDto> Statistics => _scanner.Statistics;
        
        public ICommand ScanToggle { get; }
        public ICommand IncExpectedRange { get; }
        public ICommand DecExpectedRange { get; }
        
        [Reactive] public string ScanText { get; private set; } = "Scan";
        [Reactive] public int ExpectedRange  { get; private set; }
        
        
        public void StartScan()
        {
            if(_scanner.IsScanning)
                return;
             
            ScanText = "Stop";
   
            try
            {
                _scanner.Start(RxApp.MainThreadScheduler);
                IsBusy = true;
            }
            catch (Exception e)
            {
                _dialogs.Alert($"{e.Message}","Ошибка сканирования" );
            }
        }
        
        
        public void StopScan()
        {
            ScanText = "Scan";
            _scanner.StopScan();
        }
    }
}