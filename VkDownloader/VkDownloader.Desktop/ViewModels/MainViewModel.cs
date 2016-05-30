using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Data;
using Prism.Interactivity.InteractionRequest;
using VkDownloader.Desktop.Notifications;
using VkNet;
using VkNet.Exception;
using VkNet.Model.RequestParams;

namespace VkDownloader.Desktop.ViewModels
{
    using System.Windows.Input;
    using Prism.Commands;
    using Prism.Mvvm;
    using Settings;

    class MainViewModel : BindableBase
    {
        private readonly ISettings _settings;
        private readonly VkApi _api = new VkApi() {RequestsPerSecond = 3};
        private readonly object _dialogsLock = new object();
        private string _name;

        public MainViewModel(ISettings settings)
        {
            _settings = settings;

            BindingOperations.EnableCollectionSynchronization(Dialogs, _dialogsLock);

            AuthInteractionRequest = new InteractionRequest<AuthNotification>();

            LoadedCommand = new DelegateCommand(ExecuteLoaded);
        }

        public string Name
        {
            get
            {
                return _name;
            }

            private set
            {
                SetProperty(ref _name, value);
            }
        }

        public ObservableCollection<DialogViewModel> Dialogs { get; } = new ObservableCollection<DialogViewModel>();

        public ICommand LoadedCommand { get; }
        
        public InteractionRequest<AuthNotification> AuthInteractionRequest { get; }

        private async void ExecuteLoaded()
        {
            if (CheckAuthentication())
            {
                ReloadDialogsAsync();
            }
            else
            {
                var notification = new AuthNotification(_settings.AppId) {};
                await AuthInteractionRequest.RaiseAsync(notification);
                if (notification.Confirmed)
                {
                    _settings.AccessToken = notification.AccessToken;
                    _api.Authorize(_settings.AccessToken);
                    _api.RequestsPerSecond = 1;
                    if (_api.IsAuthorized)
                    {
                        ReloadDialogsAsync();
                    }
                }    
            }
        }

        private bool CheckAuthentication()
        {
            if (string.IsNullOrWhiteSpace(_settings.AccessToken))
            {
                return false;
            }
            
            _api.Authorize(_settings.AccessToken);

            try
            {
                var profile = _api.Account.GetProfileInfo();
                Name = $"{profile.FirstName} {profile.LastName}";
            }
            catch (UserAuthorizationFailException ex)
            {
                return false;
            }

            return _api.IsAuthorized;
        }

        private async void ReloadDialogsAsync()
        {
            await Task.Run(() =>
            {

                lock (_dialogsLock)
                {
                    Dialogs.Clear();
                    var dialogs = _api.Messages.GetDialogs(new MessagesDialogsGetParams() { Count = 10 });
                    try
                    {

                        foreach (var dialog in dialogs.Messages)
                        {
                            Dialogs.Add(new DialogViewModel(_api, dialog));
                        }

                    }
                    catch (Exception)
                    {

                    }
                }
            });
        }
    }
}
