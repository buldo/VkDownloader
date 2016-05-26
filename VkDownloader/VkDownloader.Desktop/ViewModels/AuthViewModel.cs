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
using VkDownloader.Settings;

namespace VkDownloader.Desktop.ViewModels
{
    class AuthViewModel : BindableBase, IInteractionRequestAware

    {
        #region Fields

        private readonly DelegateCommand _okCommand;
        private readonly DelegateCommand _cancelCommand;
        private AuthNotification _notification;

        #endregion // Fields

        public AuthViewModel()
        {
            _okCommand = new DelegateCommand(ExecuteOk);
            _cancelCommand = new DelegateCommand(ExecuteCancel);
        }
        
        public string Login { get; set; }

        public string Password { get; set; }

        public ICommand OkCommand => _okCommand;

        public ICommand CancelCommand => _cancelCommand;

        private void ExecuteOk()
        {
            
        }

        private void ExecuteCancel()
        {
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
            }
        }

        public Action FinishInteraction { get; set; }
    }
}
