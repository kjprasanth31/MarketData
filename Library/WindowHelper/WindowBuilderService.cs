using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Library.WindowHelper
{
    public class WindowBuilderService : IWindowBuilderService
    {
        private object _dataContext;
        public string _title;
        private object _userControl;
        private WindowStyle? _style;

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

            if (_style != WindowStyle.ToolWindow)
                window.Icon = BitmapFrame.Create(iconUri);

            window.Height = 375;
            window.Width = 567;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Background = Brushes.DarkGray;
            window.Content = _userControl;
            window.WindowStyle = _style ?? WindowStyle.SingleBorderWindow;
            window.BorderBrush = Brushes.SlateGray;
            window.BorderThickness = new Thickness(1,0,1,1);
            window.Show();
        }

        public IWindowBuilderService WindoStyle(WindowStyle? style)
        {
            _style = style;
            return this;
        }
    }

    public interface IWindowBuilderService
    {
        IWindowBuilderService DataContext(object context);
        IWindowBuilderService Title(string title);
        IWindowBuilderService View(UserControl userControl);
        IWindowBuilderService WindoStyle(WindowStyle? style);
        void Show();
    }
}