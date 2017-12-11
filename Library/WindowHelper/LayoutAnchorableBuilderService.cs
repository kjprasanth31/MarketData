using System;
using System.ComponentModel;
using System.Windows.Controls;
using NLog;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;

namespace Library.WindowHelper
{
    public class LayoutAnchorableBuilderService : ILayoutAnchorableBuilderService
    {
        private string _title;
        private object _dataContext;
        private UserControl _userControl;
        private DockingManager _dockingManager;
        private readonly NLog.Logger _logger;
        private IDisposable _disposable;

        public LayoutAnchorableBuilderService()
        {
            _logger = LogManager.GetLogger(nameof(LayoutAnchorableBuilderService));
        }
        public ILayoutAnchorableBuilderService Title(string title)
        {
            _title = title;
            return this;
        }
        public ILayoutAnchorableBuilderService DataContext(object dataContext)
        {
            _dataContext = dataContext;
            return this;
        }

        public ILayoutAnchorableBuilderService View(UserControl userControl)
        {
            _userControl = userControl;
            return this;
        }

        public ILayoutAnchorableBuilderService OnClosed(IDisposable disposable)
        {
            _disposable = disposable;
            return this;
        }

        public ILayoutAnchorableBuilderService DockingManager(DockingManager dockingManager)
        {
            _dockingManager = dockingManager;
            return this;
        }

        public void Show()
        {
            if (_dockingManager != null)
            {
                var layout = new LayoutAnchorable();
                layout.Closing += LayoutOnClosing;
                layout.AddToLayout(_dockingManager, AnchorableShowStrategy.Most);
                layout.Title = _title;
                if (_userControl != null)
                {
                    _userControl.DataContext = _dataContext;
                    layout.Content = _userControl;
                }
                layout.FloatingHeight = 250;
                layout.FloatingWidth = 345;
                layout.Float();
                layout.Closing += LayoutOnClosing;
                _logger.Info($"Launching LayoutAnchorable Window - Type : {_userControl?.GetType()}, Title : {_title}, DataContext : {_dataContext?.GetType()}");
            }
        }
        

        private void LayoutOnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            _disposable?.Dispose();
        }
    }

    public interface ILayoutAnchorableBuilderService
    {
        ILayoutAnchorableBuilderService DataContext(object context);
        ILayoutAnchorableBuilderService Title(string title);
        ILayoutAnchorableBuilderService View(UserControl userControl);
        ILayoutAnchorableBuilderService DockingManager(DockingManager dockingManager);
        ILayoutAnchorableBuilderService OnClosed(IDisposable disposable);
        void Show();
    }
}
