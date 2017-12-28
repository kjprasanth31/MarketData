using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using Library.Extensions;
using Library.Nats;

namespace MarketDataController
{
    public class FxViewController : IDisposable
    {
        private readonly FxViewModel _viewModel;
        private readonly NatsTransport<CurrencyPairPrice> _natsTransport;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        public FxViewController(NatsTransport<CurrencyPairPrice> natsTransport)
        {
            _natsTransport = natsTransport;
            _viewModel = new FxViewModel();
            Initialise();
        }

        private void Initialise()
        {

            _natsTransport
                .ObserveCurrency()
                .ObserveOn(Scheduler.Default)
                .Distinct(x => x.ToString())
                .Subscribe(x =>
                {
                    Application.Current.Dispatcher.Invoke(() => _viewModel.CurrencyPairList.Add(new CurrencyPairItem(x.ToString(), x.ToString())));
                })
                .AddToDispose(_disposable);
                
        }

        public FxViewModel ViewModel => _viewModel;

        public void Dispose()
        {
            _natsTransport.Disconnect();
            _disposable.Dispose();
        }
    }
}
