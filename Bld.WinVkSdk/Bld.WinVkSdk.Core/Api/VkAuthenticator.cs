namespace Bld.WinVkSdk.Core.Api
{
    using System;
    using RestSharp;
    using RestSharp.Authenticators;

    internal class VkAuthenticator : IAuthenticator
    {
        private readonly Func<string> _tokenAccessFunc;

        public VkAuthenticator(Func<string> tokenAccessFunc)
        {
            _tokenAccessFunc = tokenAccessFunc;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            var token = _tokenAccessFunc();

            request.AddParameter("access_token", token, ParameterType.GetOrPost);
        }
    }
}
