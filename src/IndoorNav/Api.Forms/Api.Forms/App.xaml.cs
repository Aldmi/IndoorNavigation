using Api.Forms.Pages.Beacons;
using Api.Forms.Pages.BluetoothLE;
using Api.Forms.Pages.Logging;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms;
using beacons = Api.Forms.Pages.Beacons;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Api.Forms
{
    public partial class App 
    {
        public App() : this(null) { }
        public App(IPlatformInitializer initializer) : base(initializer)
        {
        }
        
        protected override IContainerExtension CreateContainerExtension() =>
            PrismContainerExtension.Current;
        
        protected override async void OnInitialized()
        {
            InitializeComponent();
            XF.Material.Forms.Material.Init(this);
            
            //var res=await NavigationService.NavigateAsync("Main/Nav/Welcome");
            //var res=await NavigationService.NavigateAsync("Nav/Main");
            //var res=await NavigationService.NavigateAsync("Nav/BleCentral");
            //var res=await NavigationService.NavigateAsync("Main/Nav/TestLogProviders/Logs");
            var res=await NavigationService.NavigateAsync("Main/Nav/ManagedBeacon");
        }
        
        
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Common
            containerRegistry.RegisterForNavigation<NavigationPage>("Nav");
            containerRegistry.RegisterForNavigation<MainPage, MainViewModel>("Main");
            //containerRegistry.RegisterForNavigation<WelcomePage>("Welcome");
            
            //Logging
            containerRegistry.RegisterForNavigation<LogPage, LogViewModel>("Logs");
            containerRegistry.RegisterForNavigation<TestLogPage, TestLogViewModel>("TestLogProviders");
            
            //Ble
            containerRegistry.RegisterForNavigation<AdapterPage, AdapterViewModel>("BleCentral");
            containerRegistry.RegisterForNavigation<PeripheralPage, PeripheralViewModel>("Peripheral");

            //Spheres
            containerRegistry.RegisterForNavigation<ManagedBeaconPage, ManagedBeaconViewModel>("ManagedBeacon");
            containerRegistry.RegisterForNavigation<beacons.CreatePage, beacons.CreateViewModel>("CreateBeacon");
            
            // containerRegistry.RegisterForNavigation<MainPage, MainViewModel>("Main");
            // containerRegistry.RegisterForNavigation<ManagedBeaconPage, ManagedBeaconViewModel>("ManagedBeacon");
            // containerRegistry.RegisterForNavigation<CreatePage, CreateViewModel>("CreateBeacon");
            // containerRegistry.RegisterForNavigation<WelcomePage>("Welcome");
        }


        protected override void OnStart()
        {
            //Run jobs here.
            base.OnStart();
        }
        
        
        protected override void OnResume()
        {
        }
    }
}