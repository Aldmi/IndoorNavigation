using System;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using Shiny;

namespace Api.Forms.Infrastructure
{
    public class GlobalExceptionHandler : IObserver<Exception>, IShinyStartupTask
    {
        private readonly IDialogs _dialogs;
        private readonly ILogger _logger;


        public GlobalExceptionHandler(IDialogs dialogs, ILogger<GlobalExceptionHandler> logger)
        {
            _dialogs = dialogs;
            _logger = logger;
        }


        public void Start() => RxApp.DefaultExceptionHandler = this;
        public void OnCompleted() {}
        public void OnError(Exception error) {}


        public async void OnNext(Exception value)
        {
            _logger.LogError(value, "Error in view caught");
            await _dialogs.Alert(value.ToString(), "ERROR");
        }
    }
}