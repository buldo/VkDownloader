namespace Bld.WinVkSdk.Core.Api
{
    using System.Collections.Generic;

    using Bld.WinVkSdk.Core.Data;
    
    using RestSharp;

    public class FriendsApi : BaseCollectionApi<VkUserData>
    {
        internal FriendsApi(ApiRequester requester)
            : base(requester)
        {  
        }

        public List<VkUserData> Get()
        {
            return RequestElementsFromServer(CreateGetRequest());
        }

        protected List<VkUserData> RequestElementsFromServer(RestRequest request)
        {
            var responces = Requester.Execute<List<VkResponse<VkUserData>>>(request);
            return responces[0].Items;
        }

        private RestRequest CreateGetRequest()
        {
            var request = new RestRequest
            {
                Resource = "friends.get"

            };
            
            //request.AddParameter("user_id", CurrenUser.Id, ParameterType.GetOrPost);
            request.AddParameter("fields", "name", ParameterType.GetOrPost);
            request.AddParameter("order", "hints", ParameterType.GetOrPost);

            return request;
        }
    }
}
