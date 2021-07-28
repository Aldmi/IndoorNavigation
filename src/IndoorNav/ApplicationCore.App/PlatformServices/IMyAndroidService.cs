using System.Collections.Generic;

namespace ApplicationCore.App.PlatformServices
{
    public interface IMyAndroidService
    {
        List<int> GetAll();
        string Name { get; }
    }
}