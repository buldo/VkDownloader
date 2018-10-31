namespace Bld.WinVkSdk.Core.Responces
{
    using System.Collections.Generic;

    using Data;

    public class VkAttachmentsResponce
    {
        public List<AttachmentItem> Items { get; internal set; }

        public string NextFrom { get; internal set; }

        public enum Type
        {
            Photo,
            Video,
            Audio,
            Doc,
            Link,
            Market,
            Wall,
            Share
        }

        public class AttachmentItem
        {
            public Type Type { get; internal set; }

            public VkPhotoData Photo { get; internal set; }
            //Video,
            //Audio,
            //Doc,
            //Link,
            //Market,
            //Wall,
            //Share
        }
    }
}
