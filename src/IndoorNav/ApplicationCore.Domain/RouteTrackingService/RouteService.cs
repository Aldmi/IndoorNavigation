using System;
using System.Reactive.Linq;
using System.Threading;
using ApplicationCore.Domain.CheckPointModel;
using ApplicationCore.Domain.MovingService.Model;
using ApplicationCore.Domain.RouteTrackingService.Model;

namespace ApplicationCore.Domain.RouteTrackingService
{
    public class RouteService
    {
        private readonly IRouteBuilder _builder;
        private readonly IRouteTracker _tracker;
        private CancellationTokenSource? _cts;

        
        public RouteService(IRouteBuilder builder, IRouteTracker tracker)
        {
            _builder = builder;
            _tracker = tracker;
        }
        
        
        /// <summary>
        /// Трекер запущен
        /// </summary>
        public bool IsTrakerStarted =>  !_cts?.IsCancellationRequested ?? false;
        
        /// <summary>
        /// Событие выдает новый построенный маршрут
        /// </summary>
        public IObservable<Route> BuildRouteRx => _builder.BuildRouteRx;


        /// <summary>
        /// Запустить трекер следование по маршруту.
        /// </summary>
        /// <param name="observableMoving"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IObservable<TrackingResult> StartTracker(IObservable<Moving> observableMoving, CheckPointBase start, CheckPointBase end)
        {
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
            var route = _builder.Build("", start, end);
            _tracker.SetRoute(route);
            
            var obs = observableMoving
                .Select(moving => _tracker.Control(moving))
                .Do(trackingRes =>   //TODO: првоерить DoWhile()
                {
                    if (trackingRes.State == TrackingState.LostRoute)
                    {
                        //start - новая точка (куда неверно ушли) брать из trackingRes (можно прям выделить поле если сбились с маршрута то выставлять LostRouteCheckPoint)
                        //end - не меняется
                        var routeNew = _builder.Build("", start, end);
                        _tracker.SetRoute(routeNew);
                    }
                }) 
                //генерировать последовательность пока не вызвали отмену
                .TakeUntil(_=> _cts.IsCancellationRequested)
                //генерировать последовательность пока не закончим маршрут
                .TakeUntil(tracingRes => tracingRes.State == TrackingState.CompleteRoute); 
            
            return obs;
        }

        /// <summary>
        /// Остановить трекер.
        /// </summary>
        public void StopTracker()
        {
            _cts?.Cancel();
        }
        
    }
}