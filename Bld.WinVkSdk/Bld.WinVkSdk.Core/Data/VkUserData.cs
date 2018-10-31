namespace Bld.WinVkSdk.Core.Data
{
    using RestSharp.Deserializers;

    public class VkUserData
    {
        [DeserializeAs(Name = "id")]
        public ulong Id { get; internal set; }

        [DeserializeAs(Name = "first_name")]
        public string FirstName { get; internal set; }

        [DeserializeAs(Name = "last_name")]
        public string LastName { get; internal set; }
    }
}
