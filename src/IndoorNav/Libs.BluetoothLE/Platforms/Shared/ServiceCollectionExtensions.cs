using System;
using Microsoft.Extensions.DependencyInjection;
using Shiny;
using Shiny.BluetoothLE;

namespace Libs.BluetoothLE.Platforms.Shared
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register the IBleManager service that allows you to connect to other BLE devices - Delegates used here are intended for background usage
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <param name="delegateType"></param>
        /// <returns></returns>
        public static bool UseBleClient(this IServiceCollection services, Type delegateType, BleConfiguration? config = null)
        {
            if (services.UseBleClient(config))
            {
                if (delegateType != null)
                    services.AddSingleton(typeof(IBleDelegate), delegateType);
                return true;
            }
            return false;
        }


        /// <summary>
        /// Register the ICentralManager service that allows you to connect to other BLE devices - Delegates used here are intended for background usage
        /// </summary>
        /// <typeparam name="TCentralDelegate"></typeparam>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static bool UseBleClient<TCentralDelegate>(this IServiceCollection services, BleConfiguration? config = null) where TCentralDelegate : class, IBleDelegate
            => services.UseBleClient(typeof(TCentralDelegate), config);


        /// <summary>
        /// Register the ICentralManager service that allows you to connect to other BLE devices
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static bool UseBleClient(this IServiceCollection builder, BleConfiguration? config = null)
        {
            builder.RegisterModule(new BleShinyModule(config));
            return true;
        }
    }
}
