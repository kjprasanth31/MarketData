using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace WindowHelper
{
    public class WindowBuilderService : IWindowBuilderService
    {
        private object _dataContext;
        public string _title;
        private object _userControl;

        public IWindowBuilderService DataContext(object dataContext)
        {
            _dataContext = dataContext;
            return this;
        }

        public IWindowBuilderService Title(string title)
        {
            _title = title;
            return this;
        }

        public IWindowBuilderService View(UserControl userControl)
        {
            _userControl = userControl;
            return this;
        }

        public void Show()
        {
            var window = new Window
            {
                DataContext = _dataContext,
                Title = _title?? "** Add Title **"
            };

            var iconUri = new Uri("pack://application:,,,/line-chart.ico", UriKind.RelativeOrAbsolute);
            window.Icon = BitmapFrame.Create(iconUri);
            window.Height = 375;
            window.Width = 567;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Background = Brushes.DarkGray;
            window.Content = _userControl;
            window.Show();
        }
    }

    public interface IWindowBuilderService
    {
        IWindowBuilderService DataContext(object context);
        IWindowBuilderService Title(string title);
        IWindowBuilderService View(UserControl userControl);
        void Show();
    }

    public class MayBe<T>
    {
        T _value;
        public MayBe(T value)
        {
            _value = value;
        }

        public T Value()
        {
            if (_value == null)
                return default(T);

            return _value;
        }
    }
}