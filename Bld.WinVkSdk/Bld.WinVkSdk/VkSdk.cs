using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bld.WinVkSdk
{
    using AutoMapper;

    using Bld.WinVkSdk.Cache;
    using Bld.WinVkSdk.Collections;
    using Bld.WinVkSdk.Core;
    using Bld.WinVkSdk.Core.Api;
    using Bld.WinVkSdk.Core.Data;
    using Bld.WinVkSdk.Models;

    public class VkSdk
    {
        private readonly VkClient _client;

        static VkSdk()
        {
            Mapper.Initialize(
                cfg =>
                    {
                        cfg.CreateMap<VkUserData, VkUser>();
                        cfg.CreateMap<VkPhotoData, VkPhoto>();
                    });
        }

        public VkSdk(Func<string> tokenAccessFunc, IApiCache cache)
        {
            _client = new VkClient(tokenAccessFunc);
            Friends = new FriendsCollection(_client, cache);
            Dialogs = new DialogsCollection(_client, cache);
        }

        public IFriendsCollection Friends { get; }

        public IDialogsCollection Dialogs { get; }
    }
}
