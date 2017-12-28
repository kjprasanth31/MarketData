using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using MarketDataController.Annotations;

namespace MarketDataController
{
    public class FxViewModel : INotifyPropertyChanged
    {
        public CurrencyPairItemCollection CurrencyPairList { get; }

        private string _selectedCurrencyPair;

        public string SelectedCurrencyPair
        {
            get => _selectedCurrencyPair;
            set
            {
                _selectedCurrencyPair = value;
                NotifyPropertyChanged(nameof(SelectedCurrencyPair));
            }
        }

        public FxViewModel()
        {
            CurrencyPairList = new CurrencyPairItemCollection();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CurrencyPairItemCollection : ObservableCollection<CurrencyPairItem>
    {
    }

    public class CurrencyPairItem
    {
        public string Id { get; }
        public string Name { get; }

        public CurrencyPairItem(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class FxItem
    {
        private string _id;
        private string _currency1;
        private string _currency2;
        private double _rate;
        private DateTime _updateTime;
        
        public FxItem(string id, string currency1, string currency2, double rate, DateTime updateTime)
        {
            Id = id;
            Currency1 = currency1;
            Currency2 = currency2;
            Rate = rate;
            UpdateTime = updateTime;
        }

        public string Id
        {
            get => _id;
            set
            {
                if (value != null)
                    _id = value;
            }

        }

        public string Currency1
        {
            get => _currency1;
            set
            {
                if (value != null)
                    _currency1 = value;

            }
        }

        public string Currency2
        {
            get => _currency2;
            set
            {
                if (value != null)
                    _currency2 = value;
            }
        }

        public double Rate
        {
            get => _rate;
            set => _rate = value;
        }

        public DateTime UpdateTime
        {
            get => _updateTime;
            set => _updateTime = value;
        }
    }
}
