using Prism.Commands;
using System.Windows;
using WindowHelper;

namespace MarketData
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowController().ViewModel;
        }
    }

    public class MainWindowViewModel
    {
        public DelegateCommand<string> NewWindowCommand { get; set; }
        public MainWindowViewModel()
        {
        }
    }

    public class MainWindowController
    {
        public MainWindowViewModel ViewModel;
        private readonly WindowBuilderService _windowBuilderService;

        public MainWindowController()
        {
            ViewModel = new MainWindowViewModel { NewWindowCommand = new DelegateCommand<string>(title => CreateNewWindow(title), _ => true) };
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
