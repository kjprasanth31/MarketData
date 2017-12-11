using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Runtime.CompilerServices;
using MarketDataController.Annotations;

namespace MarketDataController
{
    public class AngularGaugeViewModel : INotifyPropertyChanged
    {
        private AngularItem _selectedGaugeItem;
        private double _memoryUsage;
        private double _cpuUsage;
        private string _metric;
        private double _gaugeValue;

        public AngularGaugeViewModel()
        {
        }

        public ObservableCollection<AngularItem> GaugeItems { get; } = new ObservableCollection<AngularItem>();
        public AngularItem SelectedGaugeItem
        {
            get => _selectedGaugeItem;
            set
            {
                _selectedGaugeItem = value;
                NotifyPropertyChanged(nameof(SelectedGaugeItem));
            } 
        }

        public double GaugeValue
        {
            get => _gaugeValue;
            set
            {
                _gaugeValue = double.IsNaN(value) ? 0.0 : value;
                NotifyPropertyChanged(nameof(GaugeValue));
            }
        }

        public double MemoryUsage
        {
            get => _memoryUsage;
            set
            {
                _memoryUsage = double.IsNaN(value) ? 0.0 : value; ;
                NotifyPropertyChanged(nameof(MemoryUsage));
            }
        }

        public double CpuUsage
        {
            get => _cpuUsage;
            set
            {
                _cpuUsage = double.IsNaN(value) ? 0.0 : value; ;
                NotifyPropertyChanged(nameof(CpuUsage));
            }
        }

        public string Metric
        {
            get => _metric;
            set
            {
                _metric = value;
                NotifyPropertyChanged(nameof(Metric));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class AngularItem : INotifyPropertyChanged
    {
        private string _id;
        private string _name;
        private readonly Func<IDisposable> _getData;

        public AngularItem(string id, string name, [CanBeNull] Func<IDisposable> getData)
        {
            _id = id;
            _name = name;
            _getData = getData ?? (() => Disposable.Empty);
        }

        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                    NotifyPropertyChanged(nameof(Id));

            }
        }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NotifyPropertyChanged(nameof(Name));

            }
        }

        public Func<IDisposable> GetData => _getData;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
