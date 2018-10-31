namespace Bld.WinVkSdk.Cache
{
    using System.Collections.Generic;

    using Models;

    public interface IApiCache
    {
        List<VkUser> GetFriends();

        VkUser GetUserById(ulong id);

        void Update(VkUser user);

        void UpdateFriends(List<VkUser> friends);
    }
}
