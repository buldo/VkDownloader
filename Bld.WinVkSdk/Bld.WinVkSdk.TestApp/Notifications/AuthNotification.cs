namespace Bld.WinVkSdk.TestApp.Notifications
{
    using Prism.Interactivity.InteractionRequest;

    public class AuthNotification : Confirmation
    {
        public AuthNotification(string appId)
        {
            Title = "Login";
            ApplicationId = appId;
            AccessToken = string.Empty;
        }

        public string ApplicationId { get; }

        public string AccessToken { get; set; }
    }
}
