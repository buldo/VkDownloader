using System.Windows;
using Prism.Ioc;
using Prism.Unity;
using VkDownloader.Desktop.Settings;
using VkDownloader.Desktop.Views;

namespace VkDownloader.Desktop
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<ISettings>(new Settings.Settings());
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }
    }
}
