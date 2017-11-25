using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace WindowHelper
{
    public class WindowBuilderService : IWindowBuilderService
    {
        private Window _window;
        private object _dataContext;
        public string _title;
        private object _userControl;

        public IWindowBuilderService DataContext(object context)
        {
            _dataContext = context;
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
            _window = new Window
            {
                DataContext = _dataContext,
                Title = _title?? "** Add Title **"
            };

            var iconUri = new Uri("pack://application:,,,/line-chart.ico", UriKind.RelativeOrAbsolute);
            _window.Icon = BitmapFrame.Create(iconUri); ;
            _window.Height = 375;
            _window.Width = 567;
            _window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            _window.Background = Brushes.DarkGray;
            _window.Content = _userControl;
            _window.Show();
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