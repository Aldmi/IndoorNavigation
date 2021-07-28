using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Shiny;

namespace Api.Forms.Infrastructure
{
    public abstract class AbstractLogViewModel<TItem> : ViewModel
    {
        private readonly object syncLock = new object();


        protected AbstractLogViewModel(IDialogs dialogs)
        {
            Dialogs = dialogs;

            Logs = new ObservableList<TItem>();
            Load = ReactiveCommand.CreateFromTask(async () =>
            {
                var logs = await LoadLogs();
                Logs.ReplaceAll(logs);
            });
            Clear = ReactiveCommand.CreateFromTask(DoClear);
            BindBusyCommand(Load);

            this.WhenAnyValue(x => x.SelectedItem)
                .WhereNotNull()
                .SubscribeAsync(x => OnSelected(x))
                .DisposedBy(DestroyWith);
        }


        protected virtual Task OnSelected(TItem item) => Task.CompletedTask;

        protected IDialogs Dialogs { get; }
        public ObservableList<TItem> Logs { get; }
        [Reactive] public TItem SelectedItem { get; set; }
        public ICommand Load { get; }
        public ICommand Clear { get; }


        public override void OnAppearing()
        {
            base.OnAppearing();
            Load.Execute(null);
        }


        protected abstract Task<IEnumerable<TItem>> LoadLogs();
        protected abstract Task ClearLogs();
        protected virtual void InsertItem(TItem item)
        {
            lock (syncLock)
                Logs.Insert(0, item);
        }


        protected virtual async Task DoClear()
        {
            var confirm = await Dialogs.Confirm("Clear Logs?");
            if (confirm)
            {
                await ClearLogs();
                Load.Execute(null);
            }
        }
    }
}
