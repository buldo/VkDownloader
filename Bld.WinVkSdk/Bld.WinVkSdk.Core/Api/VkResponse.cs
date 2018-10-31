namespace Bld.WinVkSdk.Core.Api
{
    using System.Collections.Generic;
    using RestSharp.Deserializers;
    
    internal class VkResponse<T>
    {
        [DeserializeAs(Name = "count")]
        public int Count { get; set; }

        [DeserializeAs(Name = "items")]
        public List<T> Items { get; set; }
    }
}
