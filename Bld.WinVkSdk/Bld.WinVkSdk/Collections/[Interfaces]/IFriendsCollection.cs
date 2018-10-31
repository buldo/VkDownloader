namespace Bld.WinVkSdk.Collections
{
    using System.Collections.Generic;

    using Bld.WinVkSdk.Models;

    public interface IFriendsCollection
    {
        List<VkUser> Get();
    }
}