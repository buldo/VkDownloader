using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using VkDownloader.Desktop.Notifications;

namespace VkDownloader.Desktop.ViewModels
{
    internal class AuthViewModel : BindableBase, IInteractionRequestAware
    {
        #region Fields

        private readonly DelegateCommand _okCommand;
        private readonly DelegateCommand _cancelCommand;

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
                SetProperty(ref _notification, (AuthNotification) value);
                AppId = _notification.ApplicationId;
            }
        }

        public Action FinishInteraction { get; set; }
    }
}
