using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bld.WinVkSdk.Collections
{
    using Bld.WinVkSdk.Cache;
    using Bld.WinVkSdk.Core;
    using Bld.WinVkSdk.Models;

    internal class DialogsCollection : BaseCollection, IDialogsCollection
    {
        public DialogsCollection(VkClient client, IApiCache cache)
            : base(client, cache)
        {

        }

        public List<VkDialog> GetDialogs()
        {
            var messages = Client.Messages.GetDialogs(0, 100);
            var dialogs = new List<VkDialog>();
            foreach (var dialogData in messages.Items)
            {
                var dialog = new VkDialog(dialogData.Message, Cache, Client);
                
                dialogs.Add(dialog);
            }

            return dialogs;
        }
    }
}
