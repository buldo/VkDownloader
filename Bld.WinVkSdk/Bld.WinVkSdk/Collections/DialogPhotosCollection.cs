namespace Bld.WinVkSdk.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Collections;

    using AutoMapper;

    using Core;
    using Core.Data;
    using Core.Responces;
    using Models;

    internal class DialogPhotosCollection : IEnumerable<VkPhoto>
    {
        private const int ReqElementsCnt = 199;
        private readonly VkClient _client;
        private readonly ulong _peerId;
        
        public DialogPhotosCollection(VkMessageData dialogBase, VkClient client)
        {
            _client = client;
            _peerId = dialogBase.ChatId + 2000000000 ?? (ulong)Math.Abs(dialogBase.UserId);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<VkPhoto> GetEnumerator()
        {
            string startFrom = string.Empty;
            while (true)
            {
                var responce = GetNext(startFrom);
                foreach (var attachmentData in responce.Items)
                {
                    yield return Mapper.Map<VkPhoto>(attachmentData.Photo);
                }

                if (responce.Items.Count < ReqElementsCnt)
                {
                    break;
                }

                startFrom = responce.NextFrom;
            }
        }

        private VkAttachmentsResponce GetNext(string startFrom)
        {
            return _client.Messages.GetHistoryAttachments(_peerId, VkAttachmentsResponce.Type.Photo, startFrom, ReqElementsCnt);
        }
    }
}
