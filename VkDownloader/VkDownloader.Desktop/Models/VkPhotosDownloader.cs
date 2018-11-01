using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Model.Attachments;

namespace VkDownloader.Desktop.Models
{
    using System.IO;
    using System.Net;
    using System.Threading;

    internal class VkPhotosDownloader
    {
        private readonly IEnumerable<string> _photos;
        private readonly string _folder;
        private readonly List<string> _photosUrls = new List<string>();


        public VkPhotosDownloader(IEnumerable<string> photos, string folder)
        {
            _photos = photos;
            _folder = folder;
        }

        public int PhotosCount => _photosUrls.Count;

        public event EventHandler<EventArgs> PhotosCountUpdated;

        public async Task DownloadAsync(IProgress<int> progress, CancellationToken ct)
        {
            await Task.Run(
                () =>
                    {
                        foreach (var photo in _photos)
                        {
                            //_photosUrls.Add(GetMaxImageUrl(photo));
                            _photosUrls.Add(photo);
                        }
                        OnPhotosCountUpdated();
                    });

            await Task.Run(
                () =>
                    {

                        var client = new WebClient();
                        int i = 0;
                        foreach (var photoUrl in _photosUrls)
                        {
                            if (ct.IsCancellationRequested)
                            {
                                return;
                            }

                            client.DownloadFile(photoUrl, Path.Combine(_folder, i.ToString("D5") + "." + photoUrl.Split('.').Last()));

                            progress.Report(++i);
                        }

                    }, ct);
        }

        //private static string GetMaxImageUrl(Photo photo)
        //{
        //    if (!string.IsNullOrWhiteSpace(photo.))
        //    {
        //        return photo.Photo2560;
        //    }

        //    if (!string.IsNullOrWhiteSpace(photo.Photo1280))
        //    {
        //        return photo.Photo1280;
        //    }

        //    if (!string.IsNullOrWhiteSpace(photo.Photo807))
        //    {
        //        return photo.Photo807;
        //    }

        //    if (!string.IsNullOrWhiteSpace(photo.Photo604))
        //    {
        //        return photo.Photo604;
        //    }

        //    if (!string.IsNullOrWhiteSpace(photo.Photo130))
        //    {
        //        return photo.Photo130;
        //    }

        //    return photo.Photo75;
        //}

        protected virtual void OnPhotosCountUpdated()
        {
            PhotosCountUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}
