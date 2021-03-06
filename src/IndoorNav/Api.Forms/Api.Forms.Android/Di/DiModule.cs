using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Api.Forms.Android.Di;
using ApplicationCore.App.PlatformServices;
using Microsoft.Extensions.DependencyInjection;
using Shiny;

namespace Api.Forms.Android.Di
{
    public class DiModule : ShinyModule
    {
        public override void Register(IServiceCollection services)
        {
            services.AddSingleton<IStorage, AndroidStorage>();
        }
    }

    
    public class AndroidStorage : IStorage
    {
        public AndroidStorage()
        {
            
        }
        
        public List<int> GetAll()
        {
            return new List<int>(10);
        }

        public string Name => "TEST";
    }
}