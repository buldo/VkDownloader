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
            Container.RegisterInstance<ISettings>(new Settings.Win.Settings());
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