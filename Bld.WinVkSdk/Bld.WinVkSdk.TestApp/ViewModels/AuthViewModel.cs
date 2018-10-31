namespace Bld.WinVkSdk.TestApp.ViewModels
{
    using System;
    using Notifications;
    using Prism.Commands;
    using Prism.Interactivity.InteractionRequest;
    using Prism.Mvvm;

    class AuthViewModel : BindableBase, IInteractionRequestAware

    {
        #region Fields

        private string _appId = string.Empty;
        private string _accessToken = string.Empty;

        private AuthNotification _notification;

        #endregion // Fields
        public string AppId
        {
            get
            {
                return _appId;
            }

            set
            {
                SetProperty(ref _appId, value);
            }
        }

        public string AccessToken
        {
            get
            {
                return _accessToken;
            }

            set
            {
                SetProperty(ref _accessToken, value);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _notification.Confirmed = true;
                    _notification.AccessToken = value;
                    FinishInteraction();
                }
            }
        }

        private void ExecuteCancel()
        {
            _notification.Confirmed = false;
            FinishInteraction.Invoke();
        }

        public INotification Notification
        {
            get
            {
                return _notification;
            }

            set
            {
                SetProperty(ref _notification, (AuthNotification)value);
                AppId = _notification.ApplicationId;
            }
        }

        public Action FinishInteraction { get; set; }
    }
}
