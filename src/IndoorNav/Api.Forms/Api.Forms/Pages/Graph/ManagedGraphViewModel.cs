using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Api.Forms.Infrastructure;
using ApplicationCore.Domain.DiscreteSteps;
using Prism.Navigation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using UseCase.DiscreteSteps.Managed;

namespace Api.Forms.Pages.Graph
{
    public class ManagedGraphViewModel : ViewModel
    {
        private readonly INavigationService _navigator;
        private readonly IDialogs _dialogs;
        private readonly ManagedGraph _graphScanner;

        public ManagedGraphViewModel(ManagedGraph graphScanner, INavigationService navigator, IDialogs dialogs)
        {
            _navigator = navigator;
            _dialogs = dialogs;
            _graphScanner = graphScanner;
            
            ScanToggle = ReactiveCommand.Create(() =>
            {
                if (!_graphScanner.IsScanning)
                    StartScan();
                else
                    StopScan();
            });
        }

        public ObservableCollection<MovingDto> Movings => _graphScanner.Movings;
        public ICommand ScanToggle { get; }
        [Reactive] public string ScanText { get; private set; } = "Scan";
        
        
        public void StartScan()
        {
            if(_graphScanner.IsScanning)
                return;
             
            ScanText = "Stop Scan";
   
            try
            {
                _graphScanner.Start(RxApp.MainThreadScheduler);
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
            _graphScanner.Stop();
        }
    }
}