using System.Windows;
using Microsoft.Practices.Unity;
using Prism.Unity;

namespace Bld.WinVkSdk.TestApp
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<Views.MainView>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }
    }
}
