using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkDownloader.Desktop.ViewModels
{
    using System.Threading;
    using System.Windows.Input;

    using Bld.WinVkSdk.Models;

    using Prism.Commands;
    using Prism.Mvvm;

    using VkDownloader.Desktop.Models;

    internal class DownladTaskViewModel : BindableBase
    {
        private readonly VkPhotosDownloader _downloader;
        private readonly DelegateCommand _cancelCommand;

        private int _processedPhotos;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isInProgress;

        public DownladTaskViewModel(VkPhotosDownloader downloader)
        {
            _downloader = downloader;
            _cancelCommand = new DelegateCommand(ExecuteCancel);
        }

        public ICommand CancelCommand => _cancelCommand;

        public int ProcessedPhotos
        {
            get
            {
                return _processedPhotos;
            }

            private set
            {
                SetProperty(ref _processedPhotos, value);
            }
        }

        public async Task DownloadAsync()
        {
            if (_isInProgress)
            {
                return;
            }

            _isInProgress = true;
            var progress = new Progress<int>();
            progress.ProgressChanged += ProgressOnProgressChanged;
            _cancellationTokenSource = new CancellationTokenSource();
            await _downloader.DownloadAsync(progress, _cancellationTokenSource.Token);
            _isInProgress = false;
        }

        private void ProgressOnProgressChanged(object sender, int i)
        {
            ProcessedPhotos = i;
        }

        private void ExecuteCancel()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
