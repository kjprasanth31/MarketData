using System;
using System.Reactive.Disposables;

namespace Library.Extensions
{
    public static class DisposableExtension
    {
        public static void AddToDispose(this IDisposable dispose, CompositeDisposable disposable)
        {
            disposable.Add(dispose);
        }
    }
}
