using Unity;
using System.Windows;
using Library;

namespace MarketData
{
    /// <summary>
    /// Interaction logic nfor App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Initialise();
        }

        void Initialise()
        {
            var container = new UnityContainer();
            container.RegisterInstance<ILogger>(new Logger());
            container.RegisterInstance<ISomeClass>(new SomeClass(container.Resolve<ILogger>()));
            var k = container.Resolve<ISomeClass>();
        }
    }

    public class SomeClass : ISomeClass
    {
        public SomeClass(ILogger logger)
        {
            logger.LogInfo("Some Info");
            logger.LogWarning("Some Warning");
        }
    }

    public interface ISomeClass
    {
    }
}
