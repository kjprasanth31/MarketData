using System.Windows.Controls;
using Prism.Commands;
using WindowHelper;
using Xceed.Wpf.AvalonDock;
using Library.WindowHelper;

namespace MarketData
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : UserControl
    {
        public Shell()
        {
            InitializeComponent();
            DataContext = new ShellController(MyDockingManager).ViewModel;
        }
    }

    public class ShellViewModel
    {
        public DelegateCommand<string> NewWindowCommand { get; set; }
        public DelegateCommand<string> NewLayoutAnchorable { get; set; }
    }

    public class ShellController
    {
        public ShellViewModel ViewModel;
        private readonly WindowBuilderService _windowBuilderService;
        private readonly LayoutAnchorableBuilderService _layoutAnchorableBuilderService;
        private readonly DockingManager _dockingManager;

        public ShellController(DockingManager dockingManager)
        {
            ViewModel = new ShellViewModel {
                NewWindowCommand = new DelegateCommand<string>(title => CreateNewWindow(title), _ => true),
                NewLayoutAnchorable = new DelegateCommand<string>(title => CreateLayoutAnchorable(title), _ => true)};

            _windowBuilderService = new WindowBuilderService();
            _layoutAnchorableBuilderService = new LayoutAnchorableBuilderService();
            _dockingManager = dockingManager;
        }

        private void CreateLayoutAnchorable(string title)
        {
            _layoutAnchorableBuilderService
                .Title(title)
                .DockingManager(_dockingManager)
                .Show();
        }

        private void CreateNewWindow(string title)
        {
            _windowBuilderService
                .Title(title)
                .Show();
        }
    }
}
