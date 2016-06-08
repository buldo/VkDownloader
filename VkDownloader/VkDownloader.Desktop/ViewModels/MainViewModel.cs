using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Data;

using Bld.WinVkSdk;
using Bld.WinVkSdk.Cache;

using Prism.Interactivity.InteractionRequest;

using VkDownloader.Desktop.Notifications;

namespace VkDownloader.Desktop.ViewModels
{
    using System.Windows.Input;

    using Bld.WinVkSdk.Cache.InMemory;

    using Prism.Commands;
    using Prism.Mvvm;

    using Settings;

    using VkDownloader.Desktop.Models;

    class MainViewModel : BindableBase
    {
        private readonly ISettings _settings;

        private VkSdk _sdk;

        private readonly object _dialogsLock = new object();

        private string _name;

        public MainViewModel(ISettings settings)
        {
            _settings = settings;

            BindingOperations.EnableCollectionSynchronization(Dialogs, _dialogsLock);

            AuthInteractionRequest = new InteractionRequest<AuthNotification>();
            ChooseFolderRequest = new InteractionRequest<ChooseFolderConfirmation>();

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

        public ObservableCollection<DownladTaskViewModel> DownloadTasks { get; } = new ObservableCollection<DownladTaskViewModel>();

        public ICommand LoadedCommand { get; }

        public InteractionRequest<AuthNotification> AuthInteractionRequest { get; }

        public InteractionRequest<ChooseFolderConfirmation> ChooseFolderRequest { get; }

        private void ExecuteLoaded()
        {
            _sdk = new VkSdk(GetTokenAsync, new InMemoryStorage());

            lock (_dialogsLock)
            {
                foreach (var dialog in _sdk.Dialogs.GetDialogs())
                {
                    var dialogViewModel = new DialogViewModel(dialog);
                    dialogViewModel.DownloadRequested += DialogViewModelOnDownloadRequested;
                    Dialogs.Add(dialogViewModel);
                }
            }
        }

        private async void DialogViewModelOnDownloadRequested(object sender, EventArgs eventArgs)
        {
            var dialogVm = (DialogViewModel)sender;
            var confirmation = new ChooseFolderConfirmation();
            await ChooseFolderRequest.RaiseAsync(confirmation);
            if (confirmation.Confirmed)
            {
                DownloadTasks.Add(new DownladTaskViewModel(new VkPhotosDownloader(dialogVm.Dialog)));
            }
        }

        // private bool CheckAuthentication()
        // {
        // if (string.IsNullOrWhiteSpace(_settings.AccessToken))
        // {
        // return false;
        // }

        // _api.Authorize(_settings.AccessToken);

        // try
        // {
        // var profile = _api.Account.GetProfileInfo();
        // Name = $"{profile.FirstName} {profile.LastName}";
        // }
        // catch (UserAuthorizationFailException ex)
        // {
        // return false;
        // }

        // return _api.IsAuthorized;
        // }

        // private async void ReloadDialogsAsync()
        // {
        // await Task.Run(() =>
        // {

        // lock (_dialogsLock)
        // {
        // Dialogs.Clear();
        // var dialogs = _api.Messages.GetDialogs(new MessagesDialogsGetParams() { Count = 10 });
        // try
        // {

        // foreach (var dialog in dialogs.Messages)
        // {
        // Dialogs.Add(new DialogViewModel(_api, dialog));
        // }

        // }
        // catch (Exception)
        // {

        // }
        // }
        // });
        // }
        private async Task<string> GetTokenAsync()
        {
            if (string.IsNullOrWhiteSpace(_settings.AccessToken))
            {
                var notification = new AuthNotification(_settings.AppId);
                var request = await AuthInteractionRequest.RaiseAsync(notification);
                if (request.Confirmed)
                {
                    return _settings.AccessToken = request.AccessToken;
                }
            }

            return _settings.AccessToken;
        }
    }
}