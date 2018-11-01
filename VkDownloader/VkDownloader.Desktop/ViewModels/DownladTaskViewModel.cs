using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkDownloader.Desktop.ViewModels
{
    using System.Threading;
    using System.Windows.Input;

    using Prism.Commands;
    using Prism.Mvvm;

    using VkDownloader.Desktop.Models;

    internal class DownladTaskViewModel : BindableBase
    {
        private readonly VkPhotosDownloader _downloader;
        private readonly DelegateCommand _cancelCommand;

        private int _processedPhotos;
        private int _toProcessPhotosCnt;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isInProgress;

        private string _status;

        public DownladTaskViewModel(string title, VkPhotosDownloader downloader)
        {
            Title = title;
            _downloader = downloader;
            _downloader.PhotosCountUpdated += DownloaderOnPhotosCountUpdated;
            _cancelCommand = new DelegateCommand(ExecuteCancel);
        }

        public ICommand CancelCommand => _cancelCommand;

        public int ProcessedPhotosCnt
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

        public int ToProcessPhotosCnt
        {
            get
            {
                return _toProcessPhotosCnt;
            }

            private set
            {
                SetProperty(ref _toProcessPhotosCnt, value);
            }
        }

        public string Title { get; }

        public string Status
        {
            get
            {
                return _status;
            }

            private set
            {
                SetProperty(ref _status, value);
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
            Status = "Completed";
        }

        private void ProgressOnProgressChanged(object sender, int i)
        {
            ProcessedPhotosCnt = i;

            Status = $"{ProcessedPhotosCnt}/{ToProcessPhotosCnt}";
        }

        private void ExecuteCancel()
        {
            _cancellationTokenSource.Cancel();
        }

        private void DownloaderOnPhotosCountUpdated(object sender, EventArgs eventArgs)
        {
            ToProcessPhotosCnt = _downloader.PhotosCount;
        }
    }
}
