using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Data;
using Bld.WinVkSdk.Cache;
using Bld.WinVkSdk.Models;

namespace Bld.WinVkSdk.TestApp.ViewModels
{
    using System.Windows.Input;

    using Bld.WinVkSdk.Cache.InMemory;

    using Notifications;
    using Prism.Commands;
    using Prism.Interactivity.InteractionRequest;
    using Prism.Mvvm;

    class MainViewModel : BindableBase
    {
        private readonly object _friendsLock = new object();
        private readonly object _dialogsLock = new object();
        private string _name;
        private string _cachedToken;
        private VkSdk _client;

        public MainViewModel()
        {
            AuthInteractionRequest = new InteractionRequest<AuthNotification>();

            Friends = new ObservableCollection<VkUser>();
            BindingOperations.EnableCollectionSynchronization(Friends, _friendsLock);

            Dialogs = new ObservableCollection<VkDialog>();
            BindingOperations.EnableCollectionSynchronization(Dialogs, _dialogsLock);

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

        public ObservableCollection<VkUser> Friends { get; }

        public ObservableCollection<VkDialog> Dialogs { get; }

        public ICommand LoadedCommand { get; }

        public InteractionRequest<AuthNotification> AuthInteractionRequest { get; }

        private void ExecuteLoaded()
        {
            _client = new VkSdk(GetTokenAsync, new InMemoryStorage());

            lock (_friendsLock)
            {
                foreach (var friend in _client.Friends.Get())
                {
                    Friends.Add(friend);
                }
            }

            lock (_dialogsLock)
            {
                foreach (var dialog in _client.Dialogs.GetDialogs())
                {
                    Dialogs.Add(dialog);
                }
            }
        }

        private async Task<string> GetTokenAsync()
        {
            if (string.IsNullOrWhiteSpace(_cachedToken))
            {
                var notification = new AuthNotification(AppId.Value);
                var request = await AuthInteractionRequest.RaiseAsync(notification);
                if (request.Confirmed)
                {
                    return _cachedToken = request.AccessToken;
                }
            }
            return _cachedToken;
        }
    }
}
