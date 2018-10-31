namespace Bld.WinVkSdk.Collections
{
    using Cache;
    using Core;

    internal class BaseCollection
    {
        protected readonly IApiCache Cache;
        protected readonly VkClient Client;

        public BaseCollection(VkClient client, IApiCache cache)
        {
            Client = client;
            Cache = cache;
        }
    }
}
