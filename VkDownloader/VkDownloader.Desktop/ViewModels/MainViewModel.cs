using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using VkNet.Utils;

namespace VkDownloader.Desktop.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Windows.Data;
    using System.Windows.Input;

    using Prism.Interactivity.InteractionRequest;

    using VkNet;
    using VkNet.Model;
    using VkNet.Model.RequestParams;

    using Prism.Commands;
    using Prism.Mvvm;

    using Settings;

    using VkDownloader.Desktop.Models;
    using VkDownloader.Desktop.Notifications;

    class MainViewModel : BindableBase
    {
        private readonly ISettings _settings;

        private VkApi _api;

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
            get => _name;

            private set => SetProperty(ref _name, value);
        }

        public ObservableCollection<ConversationViewModel> Dialogs { get; } = new ObservableCollection<ConversationViewModel>();

        public ObservableCollection<DownladTaskViewModel> DownloadTasks { get; } = new ObservableCollection<DownladTaskViewModel>();

        public ICommand LoadedCommand { get; }

        public InteractionRequest<AuthNotification> AuthInteractionRequest { get; }

        public InteractionRequest<ChooseFolderConfirmation> ChooseFolderRequest { get; }

        private void ExecuteLoaded()
        {
            _api = new VkApi();
            var authInfo = Auth(_api.VkApiVersion.Version);

            _api.Authorize(new ApiAuthParams()
            {
                UserId = authInfo.UserId,
                AccessToken = authInfo.Token,
                TokenExpireTime = authInfo.TokenExpireTime
            });

            var allConversations = GetAllConversations();
            var userIdsFromConversation = allConversations
                .Where(c => c.Peer.Type == ConversationPeerType.User)
                .Select(c => c.Peer.Id)
                .Take(1000);

            var usersById = _api.Users.Get(userIdsFromConversation, ProfileFields.PhotoMax | ProfileFields.FirstName | ProfileFields.LastName).ToDictionary(u => u.Id);

            var conversationViewModels = new List<ConversationViewModel>();
            foreach (var conversation in allConversations)
            {
                string title = string.Empty;
                Uri photo = null;
                if (conversation.ChatSettings != null)
                {
                    title = conversation.ChatSettings.Title;
                    photo = conversation.ChatSettings.Photo?.Photo50;
                }
                else
                {
                    if (usersById.TryGetValue(conversation.Peer.Id, out var user))
                    {
                        title = $"{user.FirstName} {user.LastName}";
                        photo = user.PhotoMax;
                    }
                }

                conversationViewModels.Add(new ConversationViewModel(conversation.Peer.LocalId, title, photo));
            }

            lock (_dialogsLock)
            {
                foreach (var conversationViewModel in conversationViewModels)
                {
                    conversationViewModel.DownloadRequested += DialogViewModelOnDownloadRequested;
                    Dialogs.Add(conversationViewModel);
                }
            }
        }

        private void DialogViewModelOnDownloadRequested(object sender, EventArgs eventArgs)
        {
            var dialogVm = (ConversationViewModel)sender;
            var confirmation = new ChooseFolderConfirmation();
            ChooseFolderRequest.Raise(confirmation, async folderConfirmation =>
            {
                if (folderConfirmation.Confirmed)
                {
                    var dtask =
                        new DownladTaskViewModel(dialogVm.Title, new VkPhotosDownloader(GetPhotos(dialogVm.Id), confirmation.FolderPath));
                    DownloadTasks.Add(dtask);
                    await dtask.DownloadAsync();
                }
            });

            string[] GetPhotos(long conversationId)
            {
                var att = _api.Messages.GetHistoryAttachments(new MessagesGetHistoryAttachmentsParams()
                {
                    Count = 100,
                    MediaType = MediaType.Photo,
                    PeerId = conversationId,
                    PhotoSizes = true,
                }, out var from);

                return new string[0];
            }
        }

        private (string Token, long UserId, int TokenExpireTime) Auth(string version)
        {
            var resetEvent = new ManualResetEventSlim(false);
            var notification = new AuthNotification(_settings.AppId, version);
            string token = null;
            long user = 0;
            int tokenExpireTime = 0;
            AuthInteractionRequest.Raise(notification, authNotification =>
            {
                if (authNotification.Confirmed)
                {
                    token = authNotification.AccessToken;
                    user = authNotification.UserId;
                    tokenExpireTime = authNotification.TokenExpireTime;
                    resetEvent.Set();
                }
            });

            resetEvent.Wait();
            return (token, user, tokenExpireTime);
        }

        private List<Conversation> GetAllConversations()
        {
            const int fetchCount = 100;
            var getParams = new GetConversationsParams()
            {
                Offset = 0,
                Count = fetchCount,
                Extended = true,
                Filter = GetConversationFilter.All,
            };

            var allConversations = new List<Conversation>();

            var getConversationsResult = _api.Messages.GetConversations(getParams);
            var conversationsCount = getConversationsResult.Count;
            allConversations.AddRange(getConversationsResult.Items.Select(cm => cm.Conversation));
            while (allConversations.Count != conversationsCount)
            {
                getParams.Offset = (ulong?)allConversations.Count;
                var res = _api.Messages.GetConversations(getParams);
                allConversations.AddRange(res.Items.Select(cm => cm.Conversation));
            }

            return allConversations;
        }


    }
}