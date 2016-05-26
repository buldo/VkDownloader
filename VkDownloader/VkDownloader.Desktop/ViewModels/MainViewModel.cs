using Prism.Interactivity.InteractionRequest;
using VkDownloader.Desktop.Notifications;

namespace VkDownloader.Desktop.ViewModels
{
    using System;
    using System.Windows.Input;
    using Prism.Commands;
    using Prism.Mvvm;
    using Settings;

    class MainViewModel : BindableBase
    {
        private readonly ISettings _settings;
        

        public MainViewModel(ISettings settings)
        {
            _settings = settings;

            AuthInteractionRequest = new InteractionRequest<AuthNotification>();

            LoadedCommand = new DelegateCommand(ExecuteLoaded);


        }

        public ICommand LoadedCommand { get; }
        
        public InteractionRequest<AuthNotification> AuthInteractionRequest { get; }

        private async void ExecuteLoaded()
        {
            //if(CheckAu())

            //var notification = new AuthNotification(_settings.AppId) {};
            //await AuthInteractionRequest.RaiseAsync(notification);
            //if (notification.Confirmed)
            //{
            //    SetTocken
            //    LoadDialogs
            //}
        }
    }
}
