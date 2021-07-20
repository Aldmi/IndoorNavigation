using System;
using Api.Forms.Infrastructure;
using ApplicationCore.App;
using ApplicationCore.App.BluetoothLE;
using ApplicationCore.App.Infrastructure;
using ApplicationCore.App.Jobs;
using ApplicationCore.App.StartupTasks;
using DryIoc.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Prism.DryIoc;
using Prism.Ioc;
using Shiny;
using Shiny.Jobs;

namespace Api.Forms
{
    public class Startup : ShinyStartup
    {
        public override void ConfigureLogging(ILoggingBuilder builder, IPlatform platform)
        {
            base.ConfigureLogging(builder, platform);
        }


        public override void ConfigureServices(IServiceCollection services, IPlatform platform)
        {
            services.UseSqliteStore();
            services.UseNotifications();
            services.AddSingleton<AppNotifications>();
            services.AddSingleton<IDialogs, Dialogs>();

            // your infrastructure
            services.AddSingleton<SampleSqliteConnection>();
            services.AddSingleton<CoreDelegateServices>();
            
            //register init jobs
            services.AddSingleton<GenerateDataJob>();
            //services.AddSingleton<HandleDataJob>();
            
            // startup tasks
            services.AddSingleton<GlobalExceptionHandler>();
            services.AddSingleton<JobLoggerTask>();
            services.AddSingleton<InitStartupTask>();
            
            
            // register all of the shiny stuff you want to use
            //services.UseBleClient<BleClientDelegate>();
            
            // var job = new JobInfo(typeof(GenerateDataJob), nameof(GenerateDataJob))
            // {
            //     Repeat = true,
            //     //BatteryNotLow = true,
            //     //DeviceCharging = this.DeviceCharging,
            //     RunOnForeground = true,
            //     RequiredInternetAccess = InternetAccess.Any
            // };
            // job.SetParameter("SecondsToRun", 3);
            // services.RegisterJob(job);
            //
            // var job2 = new JobInfo(typeof(HandleDataJob), nameof(HandleDataJob))
            // {
            //     Repeat = true,
            //     //BatteryNotLow = true,
            //     //DeviceCharging = this.DeviceCharging,
            //     //RunOnForeground = false,
            //     RequiredInternetAccess = InternetAccess.Any
            // };
            // services.RegisterJob(job2);
        }
        
        
        public override IServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            // This registers and initializes the Container with Prism ensuring
            // that both Shiny & Prism use the same container
            var containerExt = PrismContainerExtension.Current;  //использует Prism.DryIoc контейнер
            ContainerLocator.SetContainerExtension(() => containerExt);
            var container = ContainerLocator.Container.GetContainer();
            container.Populate(services);                                       //добавялем в DryIoc уже зарегестрированные севрисы.
            return container.GetServiceProvider();
        }
    }
}