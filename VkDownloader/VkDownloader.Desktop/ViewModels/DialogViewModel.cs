using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using VkNet;
using VkNet.Model;

namespace VkDownloader.Desktop.ViewModels
{
    internal class DialogViewModel : BindableBase
    {
        private Message _headMessage;
        private VkApi _api;

        public DialogViewModel(VkApi _api, Message headMessage)
        {
            this._api = _api;
            this._headMessage = headMessage;

            if (headMessage.ChatId.HasValue)
            {
                DialogName = headMessage.Title;
            }
            else
            {
                if(headMessage.UserId.HasValue && headMessage.UserId > 0)
                {
                    var user = _api.Users.Get(headMessage.UserId.Value);
                    DialogName = $"{user.FirstName} {user.LastName}";
                }
            }
        }

        public string DialogName { get; }


    }
}
