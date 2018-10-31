namespace Bld.WinVkSdk.Core.Api
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using RestSharp;
    using RestSharp.Authenticators;

    public class ApiRequester
    {
        private const string BaseUrl = "https://api.vk.com/method";
        private readonly Uri _baseUri;
        private readonly IAuthenticator _authenticator;
        private readonly string _apiVersion;

        public ApiRequester(Func<string> tokenAccessFunc, string apiVersion)
        {
            _apiVersion = apiVersion;
            _baseUri = new Uri(BaseUrl);
            _authenticator = new VkAuthenticator(tokenAccessFunc);
        }

        public T Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient
            {
                BaseUrl = _baseUri,
                Authenticator = _authenticator,
                Proxy = new WebProxy()
            };

            request.AddParameter("v", _apiVersion, ParameterType.GetOrPost); // used on every request
            request.RootElement = "response";

            var response = client.Execute<T>(request);
            if (response.ErrorException != null)
            {
                string message = "Error retrieving response.  Check inner details for more info.";
                var exception = new ApplicationException(message, response.ErrorException);
                throw exception;
            }

            return response.Data;
        }
    }
}
