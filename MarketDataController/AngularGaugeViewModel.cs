using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LiveCharts;

namespace MarketDataController
{
    public class AngularGaugeViewModel : INotifyPropertyChanged
    {
        private AngularItem _selectedGaugeItem;
        private double _memoryUsage;
        private double _cpuUsage;
        private string _metric;
        private double _gaugeValue;
        private readonly SeriesCollection _seriesCollection;
        private bool _cpuVisibility;
        private bool _memoryVisilibity;

        public AngularGaugeViewModel(SeriesCollection collection)
        {
            _seriesCollection = collection;
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

        public bool CpuVisibility
        {
            get => _cpuVisibility;
            set
            {
                _cpuVisibility = value; 
                NotifyPropertyChanged(nameof(CpuVisibility));
            }
        }

        public bool MemoryVisibility
        {
            get => _memoryVisilibity;
            set
            {
                _memoryVisilibity = value;
                NotifyPropertyChanged(nameof(MemoryVisibility));
            }
        }

        public SeriesCollection SeriesCollection => _seriesCollection;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class AngularItem : INotifyPropertyChanged
    {
        private MetricType _id;
        private string _name;
        private readonly IObservable<double> _getData;

        public AngularItem(MetricType id, string name,  IObservable<double> getData)
        {
            _id = id;
            _name = name;
            _getData = getData;
        }

        public MetricType Id
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

        public IObservable<double> GetData => _getData;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
