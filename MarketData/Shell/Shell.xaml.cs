using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using Prism.Commands;
using Xceed.Wpf.AvalonDock;
using Library.WindowHelper;
using System.Windows;
using MarketData.GUI;
using MarketDataController;
using NLog;
using NLog.Targets;
using Xceed.Wpf.AvalonDock.Layout.Serialization;

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
        public DelegateCommand<string> NewToolWindowCommand { get; set; }
        public DelegateCommand<string> NewLayoutAnchorable { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand RestartCommand { get; set; }
        public DelegateCommand OpenLogFileCommand { get; set; }
        public DelegateCommand OpenLogFolderCommand { get; set; }
        

    }

    public class ShellController
    {
        public ShellViewModel ViewModel;
        private readonly WindowBuilderService _windowBuilderService;
        private readonly LayoutAnchorableBuilderService _layoutAnchorableBuilderService;
        private readonly DockingManager _dockingManager;
        private string UserConfigFileName = @"UserConfig.txt";
        private readonly XmlLayoutSerializer _layoutSerializer;

        public ShellController(DockingManager dockingManager)
        {
            ViewModel = new ShellViewModel {
                NewWindowCommand = new DelegateCommand<string>(CreateNewWindow, _ => true),
                NewToolWindowCommand = new DelegateCommand<string>(CreateNewToolWindow, _ => true),
                NewLayoutAnchorable = new DelegateCommand<string>(CreateLayoutAnchorable, _ => true),
                SaveCommand = new DelegateCommand(Save),
                RestartCommand = new DelegateCommand(ReStart),
                OpenLogFileCommand = new DelegateCommand(OpenLogFile),
                OpenLogFolderCommand = new DelegateCommand(OpenLogFolder)
            };

            _windowBuilderService = new WindowBuilderService();
            _layoutAnchorableBuilderService = new LayoutAnchorableBuilderService();
            _dockingManager = dockingManager;

            _layoutSerializer = new XmlLayoutSerializer(_dockingManager);
            _layoutSerializer.LayoutSerializationCallback += (s, e) =>
            {
                var view = GetView(e.Model.ContentId);
                if (view != null)
                {
                    view.DataContext =
                        new FxViewModel(
                            new[]
                            {
                                new FxItem("ID1", "GBP", "USD", 1.234, DateTime.Now),
                                new FxItem("ID2", "USD", "JPY", 6.3422, DateTime.Now),
                                new FxItem("ID3", "EUR", "GBP", 0.69, DateTime.Now)
                            });
                }

                e.Content = view;
            };
            Application.Current.Exit += (x, y) => Save();
            Initialise();
        }

        private void OpenLogFile()
        {
            //Target t = LogManager.Configuration.FindTargetByName("logfile");

            //if (t is NLog.Targets.FileTarget)
            //{
            //    var list = LogManager.Configuration.AllTargets.ToList();
            //    t = list.Find(x => x.Name == "logfile" + "_wrapped");
            //}
        }

        private void OpenLogFolder()
        {
            //Process.Start(NLog.LogManager.Configuration.Variables.FirstOrDefault(x => x.Key == "FolderName").Value.ToString());
        }

        private void ReStart()
        {
            Save();
            var process = new Process
            {
                StartInfo = new ProcessStartInfo(@"C:\dev\Projects\MarketData\output\bin\Debug\MarketData.exe")
            };
            if(process.Start())
                Application.Current.MainWindow?.Close();
        }

        private void Initialise()
        {
            if(File.Exists(UserConfigFileName))
                _layoutSerializer.Deserialize(UserConfigFileName);
        }

        private void Save()
        {
            _layoutSerializer.Serialize(UserConfigFileName);
        }

        private void CreateLayoutAnchorable(string title)
        {
            var vm =
                new FxViewModel(
                    new[]
                    {
                        new FxItem("ID1", "GBP", "USD", 1.234, DateTime.Now),
                        new FxItem("ID2", "USD", "JPY", 6.3422, DateTime.Now),
                        new FxItem("ID3", "EUR", "GBP", 0.69, DateTime.Now)
                    });
            
            _layoutAnchorableBuilderService
                .Title("  " + title)
                .View(GetView(title))
                .DataContext(vm)
                .DockingManager(_dockingManager)
                .Show();
        }

        private static UserControl GetView(string item)
        {
            return item != null && item.ToLower().Contains("fx") ? new FxView() : null;
        }

        private void CreateNewWindow(string title)
        {
            _windowBuilderService
                .Title(title)
                .Show();
        }

        private void CreateNewToolWindow(string title)
        {
            _windowBuilderService
                .WindoStyle(WindowStyle.ToolWindow)
                .Title(title)
                .Show();
        }
    }
}
