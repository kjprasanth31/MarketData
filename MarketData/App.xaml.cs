using System.Windows;
using Unity;
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
        }
    }
}
