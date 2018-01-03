using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using Library.Extensions;
using LiveCharts;
using LiveCharts.Wpf;

namespace MarketDataController
{
    public class AngularGaugeViewController : IDisposable
    {
        private readonly AngularGaugeViewModel _viewModel;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private CompositeDisposable _metricDisposable;
        private readonly PerformanceCounter _memCounter;
        private readonly PerformanceCounter _cpuCounter;
        private readonly TimeSpan _interval = TimeSpan.FromMilliseconds(250);

        public AngularGaugeViewController()
        {
            _viewModel = new AngularGaugeViewModel(new SeriesCollection { new LineSeries { AreaLimit = -10, Values = new ChartValues<double>() } });
            var process = Process.GetCurrentProcess();
            _memCounter = new PerformanceCounter("Process", "Working Set - Private", process.ProcessName);
            _cpuCounter = new PerformanceCounter("Process", "% Processor Time", process.ProcessName);
            Initialise();
        }

        private void Initialise()
        {
            _viewModel.GaugeItems.Add(new AngularItem(MetricType.Memory, "Memory usage (MB)", ObserveMemoryUsage()));
            _viewModel.GaugeItems.Add(new AngularItem(MetricType.Cpu, "Cpu usage (%)", ObserveCpuUsage()));

            _viewModel
                .ObservePropertyChanged(nameof(_viewModel.SelectedGaugeItem))
                .ObserveOn(Scheduler.Default)
                .Where(x => _viewModel.SelectedGaugeItem != null )
                .Do(_ =>
                {
                    if(_metricDisposable != null && !_metricDisposable.IsDisposed)
                        _metricDisposable.Dispose();

                    _metricDisposable = new CompositeDisposable();
                })
                .Subscribe(x =>
                {
                    _viewModel.CpuVisibility = _viewModel.SelectedGaugeItem.Id == MetricType.Cpu;
                    _viewModel.MemoryVisibility = _viewModel.SelectedGaugeItem.Id == MetricType.Memory;

                    var item = _viewModel.GaugeItems.FirstOrDefault(i => i.Id == _viewModel.SelectedGaugeItem.Id);
                    _viewModel.Metric = item?.Name;
                    
                    item?.GetData
                    .ObserveOn(Scheduler.Default)
                    .Subscribe(v => Application.Current.Dispatcher.Invoke(() => _viewModel.GaugeValue = v))
                    .AddToDispose(_metricDisposable);
                })
                .AddToDispose(_disposable);

            if (_viewModel.SelectedGaugeItem == null)
                _viewModel.SelectedGaugeItem = _viewModel.GaugeItems.FirstOrDefault();
        }

        private IObservable<double> ObserveMemoryUsage()
        {
            return Observable.Interval(_interval, Scheduler.Default)
                .Finally(() => _viewModel.SeriesCollection[0].Values.Clear())
                .Select(_ =>
                {
                    double value = _memCounter.NextValue() / (1024 * 1024);

                    _viewModel.SeriesCollection[0].Values.Add(value);

                    if (_viewModel.SeriesCollection[0].Values.Count > 60)
                        _viewModel.SeriesCollection[0].Values.RemoveAt(0);

                    return value;
                });
        }

        private IObservable<double> ObserveCpuUsage()
        {
            return Observable.Interval(_interval, Scheduler.Default)
                .Select(x =>
                {
                    double value =  _cpuCounter.NextValue() / Environment.ProcessorCount;
                    return value;
                });
        }

        public AngularGaugeViewModel ViewModel => _viewModel;

        public void Dispose()
        {
            _metricDisposable.Dispose();
            _disposable?.Dispose();
            _memCounter.Close();
            _memCounter.Dispose();
            _cpuCounter.Close();
            _cpuCounter.Dispose();
        }
    }

    public enum MetricType
    {
        Cpu,
        Memory
    }
}

