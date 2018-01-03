using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Windows.Controls;
using Prism.Commands;
using Xceed.Wpf.AvalonDock;
using Library.WindowHelper;
using System.Windows;
using Library.Extensions;
using Library.Nats;
using MarketData.GUI;
using MarketDataController;
using NLog;
using NLog.Layouts;
using NLog.Targets;
using NLog.Targets.Wrappers;
using Xceed.Wpf.AvalonDock.Layout.Serialization;

namespace MarketData
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : UserControl
    {
        private readonly Logger _logger = LogManager.GetLogger(nameof(Shell));

        public Shell()
        {
            _logger.Info("Initializing..");
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
        public DelegateCommand OpenFileCommand { get; set; }
        public DelegateCommand OpenFolderCommand { get; set; }
        public DelegateCommand ClearConfigCommand { get; set; }
    }

    public class ShellController
    {
        public ShellViewModel ViewModel;
        private readonly WindowBuilderService _windowBuilderService;
        private readonly LayoutAnchorableBuilderService _layoutAnchorableBuilderService;
        private readonly DockingManager _dockingManager;
        private string UserConfigFileName = @"UserConfig.txt";
        private readonly XmlLayoutSerializer _layoutSerializer;
        private readonly Logger _logger;

        private List<IDisposable> _disposable = new List<IDisposable>();
        public ShellController(DockingManager dockingManager)
        {
            _logger = LogManager.GetLogger(nameof(Shell));

            ViewModel = new ShellViewModel {
                NewWindowCommand = new DelegateCommand<string>(CreateNewWindow, _ => true),
                NewToolWindowCommand = new DelegateCommand<string>(CreateNewToolWindow, _ => true),
                NewLayoutAnchorable = new DelegateCommand<string>(CreateLayoutAnchorable, _ => true),
                SaveCommand = new DelegateCommand(Save),
                RestartCommand = new DelegateCommand(ReStart),
                OpenFileCommand = new DelegateCommand(() => Open(OpenType.LogFile)),
                OpenFolderCommand = new DelegateCommand(() => Open(OpenType.LogFolder)),
                ClearConfigCommand = new DelegateCommand(ClearConfig),
            };

            _windowBuilderService = new WindowBuilderService();
            _layoutAnchorableBuilderService = new LayoutAnchorableBuilderService();
            _dockingManager = dockingManager;

            _layoutSerializer = new XmlLayoutSerializer(_dockingManager);
            _layoutSerializer.LayoutSerializationCallback += (s, e) =>
            {
                var view = GetView(e.Model.ContentId);
                view.DataContext = GetDataContext(view.Name);
                e.Content = view;
            };

            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Closing += (x, y) =>
                {
                    Save();
                    _logger.Info($"Shutting down application with process id {Process.GetCurrentProcess().Id}..");
                };
            }

            Initialise();
        }

        private void ClearConfig()
        {
            if (File.Exists(UserConfigFileName))
            {
                try
                {
                    _logger.Info($"Clearing user config file {UserConfigFileName}..");
                    File.Delete(UserConfigFileName);
                }
                catch (Exception exception)
                {
                    _logger.Error($"Failed to delete file {UserConfigFileName}");
                    _logger.Error(exception.ToString());
                }
            }
        }

        private void Save()
        {
            _logger.Info($"Saving user config to {UserConfigFileName}..");
            _layoutSerializer.Serialize(UserConfigFileName);
        }

        private void CloseWindow()
        {
            _disposable.ForEach(x => x.Dispose());
            Application.Current.Shutdown();
        }

        private void ReStart()
        {
            _logger.Info($"Restarting application from {Application.ResourceAssembly.Location}..");
            Process.Start(Application.ResourceAssembly.Location);
            CloseWindow();
        }

        private void Open(OpenType type)
        {
            try
            {
                var config = LogManager.Configuration;
                if (config == null)
                    return;

                var target = config
                    .ConfiguredNamedTargets
                    .OfType<AsyncTargetWrapper>()
                    .Where(f => f.WrappedTarget is FileTarget)
                    .Select(f => f.WrappedTarget as FileTarget)
                    .FirstOrDefault();

                if (target == null)
                    return;

                var filename = SimpleLayout.Evaluate(target.FileName.ToString());

                if (!string.IsNullOrEmpty(filename))
                {
                    filename = filename.Replace("'", string.Empty);
                    switch (type)
                    {
                        case OpenType.LogFolder:
                            var folder = Path.GetDirectoryName(filename);
                            _logger.Info($"Opening log folder {folder}..");

                            if(folder != null)
                                Process.Start(folder);
                            return;

                        case OpenType.LogFile:
                            _logger.Info($"Opening log file {filename}..");
                            Process.Start(filename);
                            return;

                        default:
                            throw new Exception("Open Type not found");
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.Error($"Open command for {type} failed..");
                _logger.Error(exception.ToString());
            }
        }

        private void Initialise()
        {
            if (File.Exists(UserConfigFileName))
            {
                try
                {
                    using (var stream = new StreamReader(UserConfigFileName))
                    {
                        _logger.Info("User config :");
                        _logger.Info(stream.ReadToEnd);
                    }

                    _logger.Info($"Deserializing {UserConfigFileName}..");
                    _layoutSerializer.Deserialize(UserConfigFileName);
                }
                catch (Exception exception)
                {
                    _logger.Error($"Error reading user config {UserConfigFileName}");
                    _logger.Error(exception.ToString());
                }
            }
        }

        private void CreateLayoutAnchorable(string title)
        {
            _layoutAnchorableBuilderService
                .Title("  " + title)
                .View(GetView(GetItem(title)))
                .OnClosed(Disposable.Create(() => Console.WriteLine("Exited")))
                .DataContext(GetDataContext(GetItem(title)))
                .DockingManager(_dockingManager)
                .Show();
        }

        private string GetItem(string title)
        {
            return title.Contains("FX") ? "FxControl" : title.Contains("Metric") ? "AngularGaugeControl" : string.Empty;
        }

        private object GetDataContext(string viewName)
        {
            switch (viewName)
            {
                case "FxControl":
                    var transport = new NatsTransport<CurrencyPairPrice>();
                    var fvc = new FxViewController(transport);
                    _disposable.Add(fvc);
                    return  fvc.ViewModel;
                case "AngularGaugeControl":
                    var avc = new AngularGaugeViewController();
                    _disposable.Add(avc);
                    return avc.ViewModel;

                default:
                    return default(object);
            }
        }

        private static UserControl GetView(string item)
        {
            switch (item)
            {
                case "FxControl":
                    return new FxView();
                case "AngularGaugeControl":
                    return new AngularGaugeView();
                default:
                    return default(UserControl);
            }
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
                .WindowsStyle(WindowStyle.ToolWindow)
                .View(new AboutApplicationView())
                .Title(title)
                .Show();
        }
    }

    public enum OpenType
    {
        LogFile,
        LogFolder
    }
}
