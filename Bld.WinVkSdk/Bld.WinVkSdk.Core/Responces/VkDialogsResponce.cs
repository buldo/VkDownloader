namespace Bld.WinVkSdk.Core.Responces
{
    using System.Collections.Generic;

    using Data;

    public class VkDialogsResponce
    {
        public ulong Count { get; internal set; }

        public List<DialofInfo> Items { get; internal set; }

        public class DialofInfo
        {
            public ulong InRead { get; internal set; }

            public ulong OutRead { get; internal set; }

            public VkMessageData Message { get; internal set; }
        }
    }
}
