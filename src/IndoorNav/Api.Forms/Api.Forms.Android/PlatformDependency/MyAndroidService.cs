using System.Collections.Generic;
using ApplicationCore.App.PlatformServices;


namespace Api.Forms.Android.PlatformDependency
{
    public class MyAndroidService : IMyAndroidService
    {
        public MyAndroidService()
        {
            
        }
        
        public List<int> GetAll()
        {
            return new List<int>(10);
        }

        public string Name => "TEST";
    }
}