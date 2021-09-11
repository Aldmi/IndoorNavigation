namespace ApplicationCore.Domain.RouteTrackingService.Model
{
    /// <summary>
    /// Результат слежения за маршрутом.
    /// </summary>
    public class TrackingResult
    {
        private readonly TrackingState _state;
        public TrackingResult(TrackingState state)
        {
            _state = state;
        }

        public TrackingState State => _state;
    }


    public enum TrackingState
    {
        /// <summary>
        /// На маршруте
        /// </summary>
        OnRoute,
        /// <summary>
        /// ушли с маршрута
        /// </summary>
        LostRoute,
        /// <summary>
        /// маршрут перестоен
        /// </summary>
        RouteRebuilt,
        /// <summary>
        /// маршрут окончен
        /// </summary>
        CompleteRoute
    }
}