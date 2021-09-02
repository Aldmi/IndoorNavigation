using ApplicationCore.Domain.Navigation.Model;

namespace ApplicationCore.Domain.Navigation
{
    /// <summary>
    /// Слежение за маршрутом.
    /// </summary>
    public interface IRouteTracking
    {
        public Tracking Control(Moving moving);
    }
}