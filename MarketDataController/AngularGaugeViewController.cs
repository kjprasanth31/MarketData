using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Library.Extensions;

namespace MarketDataController
{
    public class AngularGaugeViewController : IDisposable
    {
        private readonly AngularGaugeViewModel _viewModel;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private IDisposable _serialDisposable = new SerialDisposable();
        private readonly Process _process;
        private readonly PerformanceCounter _memCounter;
        private readonly PerformanceCounter _cpuCounter;
        private readonly TimeSpan _interval = TimeSpan.FromMilliseconds(250);

        public AngularGaugeViewController()
        {
            _viewModel = new AngularGaugeViewModel();
            _process = Process.GetCurrentProcess();
            _memCounter = new PerformanceCounter("Process", "Working Set - Private", _process.ProcessName);
            _cpuCounter = new PerformanceCounter("Process", "% Processor Time", _process.ProcessName);

            Initialise();
        }

        private void Initialise()
        {
            _viewModel.GaugeItems.Add(new AngularItem("Threads", "Thread count", ObserveThreads));
            _viewModel.GaugeItems.Add(new AngularItem("Memory", "Memory usage (MB)", ObserveMemoryUsage));
            _viewModel.GaugeItems.Add(new AngularItem("Cpu", "Cpu usage (%)", ObserveCpuUsage));

            _viewModel
                .ObservePropertyChanged(nameof(_viewModel.SelectedGaugeItem))
                .ObserveOn(Scheduler.Default)
                .Subscribe(x =>
                {
                    var item = _viewModel.GaugeItems.FirstOrDefault(i => i.Id == _viewModel.SelectedGaugeItem.Id);
                    _viewModel.Metric = item?.Name;
                    _serialDisposable.Dispose();
                    _serialDisposable = new SerialDisposable();
                    _serialDisposable = item?.GetData.Invoke();
                })
                .AddToDispose(_disposable);

            if (_viewModel.SelectedGaugeItem == null)
                _viewModel.SelectedGaugeItem = _viewModel.GaugeItems.FirstOrDefault();
        }

        private IDisposable ObserveThreads()
        {
            return Observable.Interval(_interval,Scheduler.Default)
                .Subscribe(_ =>
                {
                    _viewModel.GaugeValue = _process.Threads.Count;
                });
        }

        private IDisposable ObserveMemoryUsage()
        {
            return Observable.Interval(_interval, Scheduler.Default)
                .Subscribe(_ =>
                {
                    _viewModel.GaugeValue = _memCounter.NextValue()/(1024 * 1024);
                });
        }

        private IDisposable ObserveCpuUsage()
        {
            return Observable.Interval(_interval, Scheduler.Default)
                .Subscribe(x =>
                {
                    _viewModel.GaugeValue = _cpuCounter.NextValue()/Environment.ProcessorCount;
                });
        }

        public AngularGaugeViewModel ViewModel => _viewModel;

        public void Dispose()
        {
            _disposable?.Dispose();
            _memCounter.Close();
            _memCounter.Dispose();
            _cpuCounter.Close();
            _cpuCounter.Dispose();
        }
    }
}

