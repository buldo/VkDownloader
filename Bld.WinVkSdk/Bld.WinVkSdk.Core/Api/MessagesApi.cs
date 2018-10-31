using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bld.WinVkSdk.Core.Api
{
    using Bld.WinVkSdk.Core.Data;
    using Bld.WinVkSdk.Core.Responces;

    using RestSharp;

    public class MessagesApi : BaseCollectionApi<VkMessageData>
    {
        public MessagesApi(ApiRequester requester)
            : base(requester)
        {
        }

        public VkDialogsResponce GetDialogs(int offset, int count)
        {
            var responces = Requester.Execute<List<VkDialogsResponce>>(GetDialogsRequest(offset, count));
            return responces[0];
        }

        public VkAttachmentsResponce GetHistoryAttachments(ulong peerId, VkAttachmentsResponce.Type mediaType, string startFrom, int count = 30)
        {
            var request = new RestRequest
            {
                Resource = "messages.getHistoryAttachments"
            };

            request.AddParameter("peer_id", peerId.ToString(), ParameterType.GetOrPost);
            request.AddParameter("media_type", mediaType.ToString().ToLower(), ParameterType.GetOrPost);
            request.AddParameter("start_from", startFrom, ParameterType.GetOrPost);
            request.AddParameter("count", count.ToString(), ParameterType.GetOrPost);

            return Requester.Execute<VkAttachmentsResponce>(request);
        }



        private RestRequest GetDialogsRequest(int offset, int count)
        {
            var request = new RestRequest
            {
                Resource = "messages.getDialogs"

            };

            //request.AddParameter("user_id", CurrenUser.Id, ParameterType.GetOrPost);
            request.AddParameter("offset", offset.ToString(), ParameterType.GetOrPost);
            request.AddParameter("count", count.ToString(), ParameterType.GetOrPost);

            return request;
        }
    }
}
