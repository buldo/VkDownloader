namespace Bld.WinVkSdk.Core.Data
{
    using RestSharp.Deserializers;

    public class VkPhotoData
    {
        [DeserializeAs(Name = "id")]
        public ulong Id { get; internal set; }

        [DeserializeAs(Name = "album_id")]
        public long AlbumId { get; internal set; }

        [DeserializeAs(Name = "owner_id")]
        public long OwnerId { get; internal set; }

        [DeserializeAs(Name = "user_id")]
        public ulong UserId { get; internal set; }

        [DeserializeAs(Name = "text")]
        public string Text { get; internal set; }

        [DeserializeAs(Name = "photo_75")]
        public string Photo75 { get; internal set; }

        [DeserializeAs(Name = "photo_130")]
        public string Photo130 { get; internal set; }

        [DeserializeAs(Name = "photo_604")]
        public string Photo604 { get; internal set; }

        [DeserializeAs(Name = "photo_807")]
        public string Photo807 { get; internal set; }

        [DeserializeAs(Name = "photo_1280")]
        public string Photo1280 { get; internal set; }

        [DeserializeAs(Name = "photo_2560")]
        public string Photo2560 { get; internal set; }

    }
}
