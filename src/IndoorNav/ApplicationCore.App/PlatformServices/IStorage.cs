using System.Collections.Generic;

namespace ApplicationCore.App.PlatformServices
{
    public interface IStorage
    {
        List<int> GetAll();
        string Name { get; }
    }
}