using System;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Bld.WinVkSdk.Wpf
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
            new FrameworkPropertyMetadata(default(string), AuthUrlPartPropertyChanged));

        public static readonly DependencyProperty TokenProperty = DependencyProperty.Register(
            "Token",
            typeof(string),
            typeof(VkAuthUserControl),
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty PermissionsStringProperty = DependencyProperty.Register(
            "PermissionsString",
            typeof(string),
            typeof(VkAuthUserControl),
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, AuthUrlPartPropertyChanged));

        public static readonly DependencyProperty ApiVersionStringProperty = DependencyProperty.Register(
            "ApiVersionString",
            typeof(string),
            typeof(VkAuthUserControl),
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, AuthUrlPartPropertyChanged));

        public static readonly DependencyProperty UserIdProperty = DependencyProperty.Register(
            "UserId",
            typeof(long),
            typeof(VkAuthUserControl),
            new FrameworkPropertyMetadata(default(long), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty TokenExpireTimeProperty = DependencyProperty.Register(
            "TokenExpireTime",
            typeof(int),
            typeof(VkAuthUserControl),
            new FrameworkPropertyMetadata(default(int), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public VkAuthUserControl()
        {
            InitializeComponent();
        }

        public string ApplicationId
        {
            get => (string)GetValue(ApplicationIdProperty);
            set => SetValue(ApplicationIdProperty, value);
        }

        public string Token
        {
            get => (string)GetValue(TokenProperty);
            set => SetValue(TokenProperty, value);
        }

        public string PermissionsString
        {
            get => (string)GetValue(PermissionsStringProperty);
            set => SetValue(PermissionsStringProperty, value);
        }

        public string ApiVersionString
        {
            get => (string)GetValue(ApiVersionStringProperty);
            set => SetValue(ApiVersionStringProperty, value);
        }

        public long UserId
        {
            get => (long)GetValue(UserIdProperty);
            set => SetValue(UserIdProperty, value);
        }

        public int TokenExpireTime
        {
            get => (int)GetValue(TokenExpireTimeProperty);
            set => SetValue(TokenExpireTimeProperty, value);
        }

        private void WebBrowserOnNavigated(object sender, NavigationEventArgs e)
        {
            if (e.Uri.AbsolutePath != "/authorize" && e.Uri.AbsoluteUri.Contains(@"oauth.vk.com/blank.html"))
            {
                string url = e.Uri.Fragment;
                url = url.Trim('#');
                var parsedUrl = HttpUtility.ParseQueryString(url);
                Token = parsedUrl.Get("access_token");
                UserId = long.Parse(parsedUrl.Get("user_id"));
                TokenExpireTime = int.Parse(parsedUrl.Get("expires_in"));
            }
        }

        private static void AuthUrlPartPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            ((VkAuthUserControl)dependencyObject).NavigateToStart();
        }

        private void NavigateToStart()
        {
            if (!string.IsNullOrWhiteSpace(ApplicationId) && !string.IsNullOrWhiteSpace(PermissionsString) && !string.IsNullOrWhiteSpace(ApiVersionString))
            {
                WebBrowser.Navigate(new Uri($"https://oauth.vk.com/authorize?client_id={ApplicationId}&display=page&redirect_uri=https://oauth.vk.com/blank.html&scope={PermissionsString}&response_type=token&v={ApiVersionString}", UriKind.Absolute));
            }
        }
    }
}
