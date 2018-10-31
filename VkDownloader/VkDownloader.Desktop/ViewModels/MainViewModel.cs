using System;
using System.Collections.ObjectModel;
using System.Threading;
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

        private string _token;

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
            _sdk = new VkSdk(GetToken, new InMemoryStorage());
            _sdk.Friends.Get();
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

        private void DialogViewModelOnDownloadRequested(object sender, EventArgs eventArgs)
        {
            var dialogVm = (DialogViewModel)sender;
            var confirmation = new ChooseFolderConfirmation();
            ChooseFolderRequest.Raise(confirmation, async folderConfirmation =>
            {
                if (folderConfirmation.Confirmed)
                {
                    var dtask =
                        new DownladTaskViewModel(dialogVm.DialogName, new VkPhotosDownloader(dialogVm.Dialog.Photos, confirmation.FolderPath));
                    DownloadTasks.Add(dtask);
                    await dtask.DownloadAsync();
                }
            });
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

        private string GetToken()
        {
            var resetEvent = new ManualResetEventSlim(true);
            if (string.IsNullOrWhiteSpace(_token))
            {
                resetEvent.Reset();
                var notification = new AuthNotification(_settings.AppId);
                AuthInteractionRequest.Raise(notification, authNotification =>
                {
                    if (authNotification.Confirmed)
                    {
                        _token = authNotification.AccessToken;
                        resetEvent.Set();
                    }
                });
            }

            resetEvent.Wait();
            return _token;
        }
    }
}