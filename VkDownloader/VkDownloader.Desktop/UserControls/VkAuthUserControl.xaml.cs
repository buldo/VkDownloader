using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VkDownloader.Desktop.UserControls
{
    /// <summary>
    /// Логика взаимодействия для VkAuthUserControl.xaml
    /// </summary>
    public partial class VkAuthUserControl : UserControl
    {
        public static readonly DependencyProperty ApplicationIdProperty = DependencyProperty.Register(
            "ApplicationId",
            typeof(string),
            typeof(VkAuthUserControl),
            new FrameworkPropertyMetadata(default(string), AppIdPropertyChangedCallback));

        public static readonly DependencyProperty TokenProperty = DependencyProperty.Register(
            "Token",
            typeof(string),
            typeof(VkAuthUserControl),
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty UserIdProperty = DependencyProperty.Register(
            "UserId",
            typeof(long),
            typeof(VkAuthUserControl),
            new FrameworkPropertyMetadata(default(long), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public VkAuthUserControl()
        {
            InitializeComponent();
        }

        public string ApplicationId
        {
            get { return (string)GetValue(ApplicationIdProperty); }
            set { SetValue(ApplicationIdProperty, value); }
        }

        public string Token
        {
            get { return (string)GetValue(TokenProperty); }
            set { SetValue(TokenProperty, value); }
        }

        public long UserId
        {
            get { return (long)GetValue(UserIdProperty); }
            set { SetValue(UserIdProperty, value); }
        }

        private void WebBrowserOnNavigated(object sender, NavigationEventArgs e)
        {
            if (e.Uri.AbsolutePath != "/authorize" && e.Uri.AbsoluteUri.Contains(@"oauth.vk.com/blank.html"))
            {
                string url = e.Uri.Fragment;
                url = url.Trim('#');
                Token = HttpUtility.ParseQueryString(url).Get("access_token");
                UserId = long.Parse(HttpUtility.ParseQueryString(url).Get("user_id"));
            }
        }

        private static void AppIdPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            ((VkAuthUserControl)dependencyObject).NavigateToStart((string)eventArgs.NewValue);
        }

        private void NavigateToStart(string appId)
        {
            if (!string.IsNullOrWhiteSpace(appId))
            {
                WebBrowser.Navigate(new Uri($"https://oauth.vk.com/authorize?client_id={appId}&display=page&redirect_uri=https://oauth.vk.com/blank.html&scope=friends,messages&response_type=token&v=5.52", UriKind.Absolute));
            }
        }
    }
}
