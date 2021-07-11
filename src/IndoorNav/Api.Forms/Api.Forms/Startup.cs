using System;
using DryIoc.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Prism.DryIoc;
using Prism.Ioc;
using Shiny;

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