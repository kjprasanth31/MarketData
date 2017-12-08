using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace MarketDataController
{
    public class FxViewController
    {
        private readonly FxViewModel _viewModel;

        public FxViewController()
        {
            _viewModel = new FxViewModel();
            Initialise();
        }

        private void Initialise()
        {
        }

        public FxViewModel ViewModel => _viewModel;
    }
}
