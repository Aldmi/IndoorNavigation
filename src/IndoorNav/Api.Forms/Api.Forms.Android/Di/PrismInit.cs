using ApplicationCore.App.PlatformServices;
using Prism;
using Prism.Ioc;

namespace Api.Forms.Android.Di
{
    public class PrismInit : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IStorage, AndroidStorage>();
        }
    }
}