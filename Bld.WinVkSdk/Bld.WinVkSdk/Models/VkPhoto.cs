namespace Bld.WinVkSdk.Models
{
    public class VkPhoto
    {
        public ulong Id { get; internal set; }

        public long AlbumId { get; internal set; }

        public long OwnerId { get; internal set; }

        public ulong UserId { get; internal set; }

        public string Text { get; internal set; }

        public string Photo75 { get; internal set; }

        public string Photo130 { get; internal set; }

        public string Photo604 { get; internal set; }

        public string Photo807 { get; internal set; }

        public string Photo1280 { get; internal set; }

        public string Photo2560 { get; internal set; }
    }
}
