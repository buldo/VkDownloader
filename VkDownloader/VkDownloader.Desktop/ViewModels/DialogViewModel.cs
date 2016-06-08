namespace VkDownloader.Desktop.ViewModels
{
    using System;
    using System.Windows.Input;

    using Prism.Mvvm;
    
    using Bld.WinVkSdk.Models;

    using Prism.Commands;

    internal class DialogViewModel : BindableBase
    {
        private readonly DelegateCommand _downloadCommand;

        public DialogViewModel(VkDialog dialog)
        {
            Dialog = dialog;
            _downloadCommand = new DelegateCommand(ExecuteDownload);
        }

        public event EventHandler<EventArgs> DownloadRequested;

        public VkDialog Dialog { get; }

        public string DialogName => Dialog.Title;

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
