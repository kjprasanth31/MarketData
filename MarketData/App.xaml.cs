using System;
using System.Collections.Generic;
using System.Linq;
using Unity;
using System.Windows;
using NLog;
using Unity.Interception.Utilities;
using ILogger = Library.ILogger;
using Logger = Library.Logger;

namespace MarketData
{
    /// <summary>
    /// Interaction logic nfor App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Dictionary<string, string> _args;
        private NLog.Logger _logger;

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            _logger = LogManager.GetLogger("App");
            GetArguments(e.Args);
            SetXceedLicense();
            
            Initialise();
        }

        private void SetXceedLicense()
        {
            Exception exception = null;
            if (_args != null && _args.TryGetValue("xceedLicense", out var license))
            {
                try
                {
                    Xceed.Wpf.Toolkit.Licenser.LicenseKey = license;
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            }
            else
            {
                exception = new Exception("Add Xceed Toolkit license as argument \"xceedLicense=YourLicense\"");
            }

            if (exception == null)
                return;

            _logger.Error(exception);

            if (MessageBox.Show(exception.ToString(), "MarketData Application Exception", MessageBoxButton.OK) == MessageBoxResult.OK)
                return;
        }

        private void GetArguments(string[] args)
        {
            if (args.Any())
            {
                _args = new Dictionary<string, string>();
                args?.ForEach(appArgs =>
                {
                    var item = appArgs.Split('=');
                    _args.Add(item[0], item[1]);
                });

                _logger.Info($"Application has {_args.Count} arguments");
            }
        }

        void Initialise()
        {
            var container = new UnityContainer();
            container.RegisterInstance<ILogger>(new Logger());
        }
    }
}
