namespace VkDownloader.Desktop.Notifications
{
    using Prism.Interactivity.InteractionRequest;

    public class AuthNotification : Confirmation
    {
        public AuthNotification(string appId, string version)
        {
            Title = "Login";
            ApplicationId = appId;
            Version = version;
        }

        public string ApplicationId { get; }

        public string AccessToken { get; set; } = string.Empty;

        public long UserId { get; set; }

        public int TokenExpireTime { get; set; }

        public string Version { get; }
    }
}