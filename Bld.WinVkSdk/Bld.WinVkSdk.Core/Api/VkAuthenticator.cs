namespace Bld.WinVkSdk.Core.Api {
    using System;
    using System.Threading.Tasks;
    using RestSharp;
    using RestSharp.Authenticators;

    internal class VkAuthenticator : IAuthenticator
    {
        private readonly Func<Task<string>> _tokenAccessFunc;

        public VkAuthenticator(Func<Task<string>> tokenAccessFunc)
        {
            _tokenAccessFunc = tokenAccessFunc;
        }

        public async void Authenticate(IRestClient client, IRestRequest request)
        {
            var tocken = await _tokenAccessFunc.Invoke();

            request.AddParameter("access_token", tocken, ParameterType.GetOrPost);
        }
    }
}
