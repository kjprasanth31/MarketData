using System.Windows.Controls;
using Prism.Commands;
using WindowHelper;

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
            DataContext = new ShellController().ViewModel;
        }
    }

    public class ShellViewModel
    {
        public DelegateCommand<string> NewWindowCommand { get; set; }
    }

    public class ShellController
    {
        public ShellViewModel ViewModel;
        private readonly WindowBuilderService _windowBuilderService;

        public ShellController()
        {
            ViewModel = new ShellViewModel { NewWindowCommand = new DelegateCommand<string>(title => CreateNewWindow(title), _ => true) };
            _windowBuilderService = new WindowBuilderService();
        }

        private void CreateNewWindow(string title)
        {
            _windowBuilderService
                .Title(title)
                .Show();
        }
    }
}
