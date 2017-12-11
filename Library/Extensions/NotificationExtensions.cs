using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;

namespace Library.Extensions
{
    public static class NotificationExtensions
    {
        //from : https://stackoverflow.com/questions/18750718/get-iobservable-from-all-property-changed-events-on-t-myproperty-in-sortedlistm
        // Betterify for later
        public static IObservable<EventPattern<PropertyChangedEventArgs>> ObserveAnyPropertyChanged(
            this INotifyPropertyChanged source)
        {
            return Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(handler => source.PropertyChanged += handler, handler => source.PropertyChanged -= handler);
        }

        public static IObservable<EventPattern<PropertyChangedEventArgs>> ObservePropertyChanged(
            this INotifyPropertyChanged source, string propertyName)
        {
            return source
                .ObserveAnyPropertyChanged()
                .Where(e => e.EventArgs.PropertyName == propertyName);
        }
    }
}
