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
        private CompositeDisposable _ccyDisposable;

        public FxViewController(NatsTransport<CurrencyPairPrice> natsTransport)
        {
            _natsTransport = natsTransport;
            _viewModel = new FxViewModel();
            Initialise();
        }

        public FxViewModel ViewModel => _viewModel;

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

            _viewModel
                .ObservePropertyChanged(nameof(_viewModel.SelectedCurrencyPair))
                .ObserveOn(Scheduler.Default)
                .Do(_ =>
                {
                    if (_ccyDisposable != null && !_ccyDisposable.IsDisposed)
                        _ccyDisposable.Dispose();

                    Application.Current.Dispatcher.Invoke(() => _viewModel.CurrencyPairPriceList.Clear());
                    _ccyDisposable = new CompositeDisposable();
                })
                .Where(x => _viewModel.SelectedCurrencyPair != null)
                .Subscribe(x =>
                {
                    _natsTransport
                    .ObserveCurrency(_viewModel.SelectedCurrencyPair.Id)
                    .ObserveOn(Scheduler.Default)
                    .Subscribe(c =>
                        {
                            Application.Current.Dispatcher.Invoke(() => _viewModel.CurrencyPairPriceList.Add(new CurrencyPairPrice(c.Currency1, c.Currency2, c.AskPrice, c.BidPrice, c.TimeStamp)));
                        })
                    .AddToDispose(_ccyDisposable);
                })
                .AddToDispose(_disposable);
        }

        public void Dispose()
        {
            _natsTransport.Dispose();
            _ccyDisposable?.Dispose();
            _disposable.Dispose();
        }
    }
}
