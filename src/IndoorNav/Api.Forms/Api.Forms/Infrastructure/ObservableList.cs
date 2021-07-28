using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive;
using System.Reactive.Linq;
using Shiny;

namespace Api.Forms.Infrastructure
{
    public class ObservableList<T> : ObservableCollection<T>
    {
        public ObservableList() { }
        public ObservableList(IEnumerable<T> items) : base(items) { }


        public IObservable<Unit> WhenCollectionChanged() => Observable.Create<Unit>(ob =>
        {
            var handler = new NotifyCollectionChangedEventHandler((sender, args) =>
                ob.Respond(Unit.Default)
            );
            CollectionChanged += handler;
            return () => CollectionChanged -= handler;
        });


        /// <summary>
        /// Adds a collection of items and then fires the CollectionChanged event - more performant than doing individual adds
        /// </summary>
        /// <param name="items"></param>
        public virtual void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
                Items.Add(item);

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items));
        }


        /// <summary>
        /// Clears and sets a new collection
        /// </summary>
        /// <param name="items"></param>
        public virtual void ReplaceAll(IEnumerable<T> items)
        {
            Clear();
            foreach (var item in items)
                Items.Add(item);

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
