namespace Bld.WinVkSdk.Collections
{
    using System.Collections.Generic;

    using Bld.WinVkSdk.Models;

    public interface IDialogsCollection
    {
        List<VkDialog> GetDialogs();
    }
}