using System;
using Api.Forms.Infrastructure;
using ApplicationCore.App;
using ApplicationCore.App.Infrastructure;
using ApplicationCore.App.Jobs;
using ApplicationCore.App.StartupTasks;
using ApplicationCore.Domain.Distance;
using ApplicationCore.Domain.Distance.Handlers;
using DryIoc.Microsoft.DependencyInjection;
using Libs.Beacons.Platforms.Shared;
using Libs.BluetoothLE.Platforms.Shared;
using Libs.Excel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prism.DryIoc;
using Prism.Ioc;
using Shiny;
using Shiny.Notifications;
using UseCase.DiscreteSteps.Managed;

namespace Api.Forms
{
    public class Startup : ShinyStartup
    {
        public override void ConfigureLogging(ILoggingBuilder builder, IPlatform platform)
        {
            base.ConfigureLogging(builder, platform);
            //builder.AddConsole(opts => opts.LogToStandardErrorThreshold = LogLevel.Debug);
            //builder.AddAppCenter("b97caf36-61ac-49b9-8985-d924c40ee36d", LogLevel.Information); //"android = b97caf36-61ac-49b9-8985-d924c40ee36d;"              //Secrets.Values.AppCenterKey;
            builder.AddSqliteLogging();
        }

        

        public override void ConfigureServices(IServiceCollection services, IPlatform platform)
        {
            //services.UseSqliteStore();
            //services.UseNotifications();
            //services.AddSingleton<AppNotifications>();
            services.AddSingleton<IDialogs, Dialogs>();

            // your infrastructure----------------------------------------------
            services.AddSingleton<SampleSqliteConnection>();
            services.AddSingleton<CoreDelegateServices>();
            services.AddScoped<IExcelAnalitic, ExcelAnalitic>();
            
            services.AddScoped<ICheckPointGraphRepository, CheckPointGraphRepository>();
            services.AddScoped<IBeaconDistanceHandler, BeaconDistanceAverageHandler>();
            
            
            //register init jobs------------------------------------------------
            //services.AddSingleton<BeaconScanJob>();
            //services.AddSingleton<HandleDataJob>();
            
            // startup tasks------------------------------------------------------
            services.AddSingleton<GlobalExceptionHandler>();
            //services.AddSingleton<JobLoggerTask>();
            //services.AddSingleton<InitStartupTask>();

            // register all of the shiny stuff you want to use---------------------
            //services.UseBleClient(); 
            services.UseBeaconRanging();


            // var job = new JobInfo(typeof(BeaconScanJob), nameof(BeaconScanJob))
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