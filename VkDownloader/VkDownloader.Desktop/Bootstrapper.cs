namespace VkDownloader.Desktop
{
    using System.Windows;

    using Microsoft.Practices.Unity;

    using Prism.Unity;

    using Settings;

    using Views;

    /// <summary>
    /// Загрузчик приложения
    /// </summary>
    internal class Bootstrapper : UnityBootstrapper
    {
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            RegisterTypeIfMissing(typeof(ISettings), typeof(Settings), true);
        }

        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainView>();
        }

        

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }
    }
}