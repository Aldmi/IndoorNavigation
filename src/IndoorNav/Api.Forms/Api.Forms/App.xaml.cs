using ApplicationCore.App;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Api.Forms
{
    public partial class App 
    {
        public App() : this(null) { }
        public App(IPlatformInitializer initializer) : base(initializer)
        {
        }
        
        // protected override IContainerExtension CreateContainerExtension() =>
        //     PrismContainerExtension.Current;
        
        
        protected override async void OnInitialized()
        {
            InitializeComponent();
            XF.Material.Forms.Material.Init(this);
            
            //var res=await NavigationService.NavigateAsync("Main/Nav/Welcome");
            var res=await NavigationService.NavigateAsync("Nav/Main");
        }
        
        
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>("Nav");
            
            containerRegistry.RegisterForNavigation<MainPage, MainViewModel>("Main");
            
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