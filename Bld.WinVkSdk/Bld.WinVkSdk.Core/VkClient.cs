namespace Bld.WinVkSdk.Core
{
    using System;
    using System.Threading.Tasks;
    using Api;

    public class VkClient
    {
        private const string ApiVersion = "5.52";

        public VkClient(Func<Task<string>> tokenAccessFunc)
        {
            var apiRequester = new ApiRequester(tokenAccessFunc, ApiVersion);

            Friends = new FriendsApi(apiRequester);
            Messages = new MessagesApi(apiRequester);
        }
        
        public FriendsApi Friends { get; }

        public MessagesApi Messages { get; }
    }
}
