namespace Bld.WinVkSdk.Core.Data
{
    using RestSharp.Deserializers;

    public class VkMessageData
    {
        [DeserializeAs(Name = "id")]
        public ulong Id { get; internal set; }

        [DeserializeAs(Name = "user_id")]
        public long UserId { get; internal set; }

        [DeserializeAs(Name = "from_id")]
        public long FromId { get; internal set; }

        [DeserializeAs(Name = "title")]
        public string Title { get; internal set; }

        [DeserializeAs(Name = "body")]
        public string Body { get; internal set; }

        [DeserializeAs(Name = "chat_id")]
        public ulong? ChatId { get; internal set; }
    }
}
