using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bld.WinVkSdk.Models
{
    using Bld.WinVkSdk.Cache;
    using Bld.WinVkSdk.Collections;
    using Bld.WinVkSdk.Core;
    using Bld.WinVkSdk.Core.Data;

    public class VkDialog
    {
        internal VkDialog(VkMessageData dialogBase, IApiCache cache, VkClient client)
        {
            if (dialogBase.ChatId.HasValue)
            {
                Title = dialogBase.Title;
            }
            else
            {
                if (dialogBase.UserId > 0)
                {
                    var user = cache.GetUserById((ulong)dialogBase.UserId);
                    if (user == null)
                    {
                        
                    }
                    else
                    {
                        Title = $"{user.FirstName} {user.LastName}";
                    }
                    
                }
            }

            Photos = new DialogPhotosCollection(dialogBase, client);
        }

        public string Title { get; internal set; }

        public IEnumerable<VkPhoto> Photos { get; }
    }
}
