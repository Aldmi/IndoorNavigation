
using Libs.BluetoothLE.Platforms.Android;
using Libs.BluetoothLE.Platforms.Android.Internals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shiny;
using Shiny.BluetoothLE;

namespace Libs.BluetoothLE.Platforms.Shared
{
    public class BleShinyModule : ShinyModule
    {
        readonly BleConfiguration config;
        public BleShinyModule(BleConfiguration config) => this.config = config ?? new BleConfiguration();


        public override void Register(IServiceCollection services)
        {
            services.AddSingleton(this.config);
            services.TryAddSingleton<ManagerContext>();
            services.TryAddSingleton<IBleManager, BleManager>();
        }
    }
}
