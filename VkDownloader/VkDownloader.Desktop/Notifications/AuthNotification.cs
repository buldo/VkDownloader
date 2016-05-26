﻿namespace VkDownloader.Desktop.Notifications
{
    using Prism.Interactivity.InteractionRequest;

    public class AuthNotification : Confirmation
    {
        public AuthNotification(string appId)
        {
            ApplicationId = appId;
            AccessToken = string.Empty;
        }

        public string ApplicationId { get; }

        public string AccessToken { get; set; }
    }
}