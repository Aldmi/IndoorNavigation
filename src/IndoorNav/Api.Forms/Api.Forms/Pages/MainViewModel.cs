using System.Windows.Input;
using Api.Forms.Infrastructure;
using Prism.Navigation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Api.Forms.Pages
{
    public class MainViewModel : ViewModel
    {
        private readonly INavigationService _navigator;
        
        public MainViewModel(INavigationService navigator) //IBleManager bleManager
        {
            _navigator = navigator;
            Title = "Main Page";
            
            Navigate = ReactiveCommand.CreateFromTask<string>(async arg =>   //ReactiveCommand.CreateFromTask разобрать как создаются команды.
            {
               IsPresented = false;
               await _navigator.NavigateAsync(arg);
            });
        }
        
        public ICommand Navigate { get; }
        [Reactive] public bool IsPresented { get; set; }
    }
}
