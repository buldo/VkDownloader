namespace Bld.WinVkSdk.Core.Api
{
    using System.Collections.Generic;
    using RestSharp;

    public abstract class BaseCollectionApi<T>
    {
        protected readonly ApiRequester Requester;
        
        protected BaseCollectionApi(ApiRequester requester)
        {
            Requester = requester;
        }
    }
}
