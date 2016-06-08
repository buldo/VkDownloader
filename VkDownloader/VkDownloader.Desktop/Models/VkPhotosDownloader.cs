using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkDownloader.Desktop.Models
{
    using System.IO;
    using System.Net;
    using System.Threading;

    using Bld.WinVkSdk.Models;

    internal class VkPhotosDownloader
    {
        private readonly IEnumerable<VkPhoto> _photos;
        private readonly string _folder;

        public VkPhotosDownloader(IEnumerable<VkPhoto> photos, string folder)
        {
            _photos = photos;
            _folder = folder;
        }
        
        public async Task DownloadAsync(IProgress<int> progress, CancellationToken ct)
        {
            await Task.Run(
                () =>
                    {
                        var photos = _photos.ToList();
                        var client = new WebClient();
                        int i = 0;
                        foreach (var photo in photos)
                        {
                            if (ct.IsCancellationRequested)
                            {
                                return;
                            }

                            var photoUrl = GetMaxImageUrl(photo);
                            client.DownloadFile(photoUrl, Path.Combine(_folder, i.ToString("D5") + "." + photoUrl.Split('.').Last()));
                            
                            progress.Report(++i);
                        }
                        
                    }, ct);
        }

        private static string GetMaxImageUrl(VkPhoto photo)
        {
            if (!string.IsNullOrWhiteSpace(photo.Photo2560))
            {
                return photo.Photo2560;
            }

            if (!string.IsNullOrWhiteSpace(photo.Photo1280))
            {
                return photo.Photo1280;
            }

            if (!string.IsNullOrWhiteSpace(photo.Photo807))
            {
                return photo.Photo807;
            }

            if (!string.IsNullOrWhiteSpace(photo.Photo604))
            {
                return photo.Photo604;
            }

            if (!string.IsNullOrWhiteSpace(photo.Photo130))
            {
                return photo.Photo130;
            }
            
            return photo.Photo75;
        }
    }
}
