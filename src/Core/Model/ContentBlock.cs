using System;

namespace Enfile.Core.Model
{
    public class ContentBlock
    {
        public ContentBlock() {}
        private ContentBlock(Guid fileID, string externalCodeId, DateTime createdAt)
        {
            this.FileID = fileID;
            this.ExternalCodeId = externalCodeId;
            this.CretedAt = createdAt;
        }

        public Guid FileID { get; set; }

        public string ExternalCodeId { get; set;}

        public int Priority { get; set; }

        public string Base64Content { get; set;}

        public DateTime CretedAt { get; set; }

        public static ContentBlock Create(Guid fileId, string externalCodeId, string base64Content, int priority, DateTime createdAt)
        {
            return new ContentBlock (fileId, externalCodeId, createdAt){
                Base64Content = base64Content,
                ExternalCodeId = externalCodeId,
                Priority = priority
            };
        }
    }
}