using System;
using System.Linq;
using Shiny;
using Shiny.Jobs;

namespace ApplicationCore.App.StartupTasks
{
    public class InitStartupTask : IShinyStartupTask
    {
        private readonly AppNotifications _appNotifications;
        private readonly IJobManager _jobManager;

        public InitStartupTask(AppNotifications appNotifications, IJobManager jobManager)
        {
            _appNotifications = appNotifications;
            _jobManager = jobManager;
        }
        
        
        public void Start()
        {
            var regs=_appNotifications.GetRegistrations()
                .Where(r=> r.IsInitStart);
            
            foreach (var r in regs)
            {
                var job = new JobInfo(r.Type, r.Name)
                {
                    //TODO: настройки запуска брать из Настроек проекта ()
                    Repeat = true, 
                    //BatteryNotLow = true,
                    //DeviceCharging = this.DeviceCharging,
                    //RunOnForeground = true,RequiredInternetAccess = InternetAccess.Any
                };
                _jobManager.Register(job);
            }
        }
    }
}