using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bld.WinVkSdk.Cache;
using Bld.WinVkSdk.Models;

namespace Bld.WinVkSdk.Collections
{
    using AutoMapper;

    using Bld.WinVkSdk.Core;

    internal class FriendsCollection : BaseCollection, IFriendsCollection
    {
        public FriendsCollection(VkClient client, IApiCache cache)
            : base(client, cache)
        {
        }

        public List<VkUser> Get()
        {
            var ret = Cache.GetFriends();
            if (ret == null)
            {
                ret = new List<VkUser>();
                var data = Client.Friends.Get();
                foreach (var userData in data)
                {
                    ret.Add(Mapper.Map<VkUser>(userData));
                }

                Cache.UpdateFriends(ret);
            }

            return ret;
        }
    }
}
