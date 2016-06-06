namespace VkDownloader.Desktop.ViewModels
{
    using System;
    using System.Windows.Input;

    using Prism.Mvvm;
    
    using Bld.WinVkSdk.Models;

    using Prism.Commands;

    internal class DialogViewModel : BindableBase
    {
        private readonly VkDialog _dialog;

        private readonly DelegateCommand _downloadCommand;

        public DialogViewModel(VkDialog dialog)
        {
            _dialog = dialog;
            _downloadCommand = new DelegateCommand(ExecuteDownload);
        }

        public event EventHandler<EventArgs> DownloadRequested;

        public string DialogName => _dialog.Title;

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
