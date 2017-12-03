using System;
using System.Collections.Generic;

namespace MarketDataController
{
    public class FxViewModel
    {
        public IEnumerable<FxItem> FxItems { get; set; }
        public FxViewModel(IEnumerable<FxItem> items)
        {
            FxItems = items;
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
