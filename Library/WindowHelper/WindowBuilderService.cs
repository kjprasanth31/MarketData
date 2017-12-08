using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NLog;

namespace Library.WindowHelper
{
    public class WindowBuilderService : IWindowBuilderService
    {
        private object _dataContext;
        public string _title;
        private object _userControl;
        private WindowStyle? _style;
        private readonly NLog.Logger _logger;

        public WindowBuilderService()
        {
            _logger = LogManager.GetLogger(nameof(WindowBuilderService));
        }

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

            window.Height = 275;
            window.Width = 377;
            if (_style != WindowStyle.ToolWindow)
            {
                window.Icon = BitmapFrame.Create(iconUri);
                window.Height = 375;
                window.Width = 567;
            }
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Background = Brushes.DarkGray;
            window.Content = _userControl;
            window.WindowStyle = _style ?? WindowStyle.SingleBorderWindow;
            window.BorderBrush = Brushes.SlateGray;
            window.BorderThickness = new Thickness(1,0,1,1);
            _logger.Info($"Launching Window - Type : {_userControl.GetType()}, Title : {_title}, Style : {_style.ToString()}, DataContext : {_dataContext?.GetType()}");
            window.Show();
        }

        public IWindowBuilderService WindowsStyle(WindowStyle? style)
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
        IWindowBuilderService WindowsStyle(WindowStyle? style);
        void Show();
    }
}