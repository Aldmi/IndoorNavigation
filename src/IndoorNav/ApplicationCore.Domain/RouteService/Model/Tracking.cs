namespace ApplicationCore.Domain.RouteService.Model
{
    /// <summary>
    /// Результат слежения за маршрутом.
    /// </summary>
    public class Tracking
    {
        private readonly TrackingState _state;
        public Tracking(TrackingState state)
        {
            _state = state;
        }
    }


    public enum TrackingState
    {
        OnRoute,
        LeftRoute
    }
}