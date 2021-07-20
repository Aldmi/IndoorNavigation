using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.AppModel;
using Prism.Navigation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Api.Forms.Infrastructure
{
    public abstract class ViewModel : ReactiveObject,
                                      IInitialize,
                                      IInitializeAsync,
                                      INavigatedAware,
                                      IPageLifecycleAware,
                                      IDestructible,
                                      IConfirmNavigationAsync
                          
    {

        CompositeDisposable? deactivateWith;
        protected CompositeDisposable DeactivateWith => deactivateWith ??= new CompositeDisposable();
        protected CompositeDisposable DestroyWith { get; } = new CompositeDisposable();


        protected virtual void Deactivate()
        {
            deactivateWith?.Dispose();
            deactivateWith = null;
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters) => Deactivate();
        public virtual void Initialize(INavigationParameters parameters) { }
        public virtual Task InitializeAsync(INavigationParameters parameters) => Task.CompletedTask;
        public virtual void OnNavigatedTo(INavigationParameters parameters) { }
        public virtual void OnAppearing() { }
        public virtual void OnDisappearing() { }
        public virtual void Destroy() => DestroyWith?.Dispose();
        public virtual Task<bool> CanNavigateAsync(INavigationParameters parameters) => Task.FromResult(true);

        [Reactive] public bool IsBusy { get; set; }
        [Reactive] public string? Title { get; protected set; }
        

        protected void BindBusyCommand(ICommand command)
            => BindBusyCommand((IReactiveCommand)command);


        protected void BindBusyCommand(IReactiveCommand command) =>
            command.IsExecuting.Subscribe(
                x => IsBusy = x,
                _ => IsBusy = false,
                () => IsBusy = false
            )
            .DisposeWith(DeactivateWith);
    }
}
