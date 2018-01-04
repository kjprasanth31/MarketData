using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Media;
using Library.Extensions;
using Library.Nats;
using LiveCharts;
using LiveCharts.Wpf;

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
            var collection = new SeriesCollection
            {
                new LineSeries
                {
                    AreaLimit = -10,
                    Values = new ChartValues<double>(),
                    Title = "Ask Price",
                    Stroke = Brushes.Blue,
                    PointGeometry = DefaultGeometries.Square
                },
                new LineSeries
                {
                    AreaLimit = -10,
                    Values = new ChartValues<double>(),
                    Title = "Bid Price",
                    Stroke = Brushes.Red,
                    PointGeometry = DefaultGeometries.Diamond
                }
            };

            _viewModel = new FxViewModel(collection);
            Initialise();
        }

        public FxViewModel ViewModel => _viewModel;

        private void Initialise()
        {
            _viewModel
                .ObservePropertyChanged(nameof(_viewModel.SelectedCurrencyPair))
                .ObserveOn(Scheduler.Default)
                .Do(_ =>
                {
                    if (_ccyDisposable != null && !_ccyDisposable.IsDisposed)
                        _ccyDisposable.Dispose();

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _viewModel.CurrencyPairPriceList.Clear();
                        _viewModel.SeriesCollection.ToList().ForEach(x => x.Values.Clear());
                    });
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
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                _viewModel.CurrencyPairPriceList.Add(new CurrencyPairPrice(c.Currency1, c.Currency2, c.AskPrice, c.BidPrice, c.TimeStamp));
                                _viewModel.SeriesCollection[0].Values.Add(double.Parse(c.AskPrice));
                                _viewModel.SeriesCollection[1].Values.Add(double.Parse(c.BidPrice));
                            });
                        })
                        .AddToDispose(_ccyDisposable);
                })
                .AddToDispose(_disposable);

            _natsTransport
                .ObserveCurrency()
                .ObserveOn(Scheduler.Default)
                .Distinct(x => x.ToString())
                .Subscribe(x =>
                {
                    var item = new CurrencyPairItem(x.ToString(), x.ToString());
                    if (_viewModel.SelectedCurrencyPair == null)
                    {
                        _viewModel.SelectedCurrencyPair = item;
                    }
                    Application.Current.Dispatcher.Invoke(() => _viewModel.CurrencyPairList.Add(item));
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
