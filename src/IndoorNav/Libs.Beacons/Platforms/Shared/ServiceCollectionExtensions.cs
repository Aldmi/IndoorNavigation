using Libs.BluetoothLE.Platforms.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;



namespace Libs.Beacons.Platforms.Shared
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register the beacon service with this if you only plan to use ranging
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static bool UseBeaconRanging(this IServiceCollection services)
        {
            services.UseBleClient();
            services.TryAddSingleton<IBeaconRangingManager, BeaconRangingManager>();
            return true;
        }


        // /// <summary>
        // /// Use this method if you plan to use background monitoring (works for ranging as well)
        // /// </summary>
        // /// <param name="services"></param>
        // /// <param name="delegateType"></param>
        // /// <returns></returns>
        // public static bool UseBeaconMonitoring(this IServiceCollection services, Type delegateType)
        // {
        //     if (delegateType == null)
        //         throw new ArgumentException("You can't register monitoring regions without a delegate type");
        //     
        //     services.TryAddSingleton<BackgroundTask>();
        //     services.UseBleClient();
        //     services.UseNotifications();
        //     return true;
        // }
        //
        //
        // /// <summary>
        // /// Use this method if you plan to use background monitoring (works for ranging as well)
        // /// </summary>
        // /// <typeparam name="T"></typeparam>
        // /// <param name="services"></param>
        // /// <returns></returns>
        // public static bool UseBeaconMonitoring<T>(this IServiceCollection services) where T : class, IBeaconMonitorDelegate
        //     => services.UseBeaconMonitoring(typeof(T));
    }
}
