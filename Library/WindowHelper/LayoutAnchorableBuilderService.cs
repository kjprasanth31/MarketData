using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
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
            }
        }
    }


    public interface ILayoutAnchorableBuilderService
    {
        ILayoutAnchorableBuilderService DataContext(object context);
        ILayoutAnchorableBuilderService Title(string title);
        ILayoutAnchorableBuilderService View(UserControl userControl);
        ILayoutAnchorableBuilderService DockingManager(DockingManager dockingManager);
        void Show();
    }
}
