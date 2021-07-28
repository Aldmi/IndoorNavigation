using System.Windows.Input;
using Xamarin.Forms;

namespace Api.Forms.Infrastructure
{
    public class ItemTappedCommandBehavior : AbstractBindableBehavior<ListView>
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ItemTappedCommandBehavior));
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }


        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);
            AssociatedObject.ItemTapped += OnItemTapped;
        }


        protected override void OnDetachingFrom(ListView bindable)
        {
            base.OnDetachingFrom(bindable);
            AssociatedObject.ItemTapped -= OnItemTapped;
        }


        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (Command == null || e.Item == null)
                return;

            if (Command.CanExecute(e.Item))
                Command.Execute(e.Item);
        }
    }
}