namespace VkDownloader.Desktop.ViewModels
{
    using System;
    using System.Windows.Input;

    using Prism.Mvvm;

    using Prism.Commands;

    internal class ConversationViewModel : BindableBase
    {
        private readonly DelegateCommand _downloadCommand;

        public ConversationViewModel(long id, string title, Uri photo)
        {
            Id = id;
            Title = title;
            Photo = photo;
            _downloadCommand = new DelegateCommand(ExecuteDownload);
        }

        public event EventHandler<EventArgs> DownloadRequested;

        public long Id { get; }

        public string Title { get; }

        public Uri Photo { get; }

        public ICommand DownloadCommand => _downloadCommand;

        private void ExecuteDownload()
        {
            OnDownloadRequested();
        }

        protected virtual void OnDownloadRequested()
        {
            DownloadRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
