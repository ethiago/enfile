using System;
using System.Threading.Tasks;
using Enfile.Commons.ApiContract.v1;
using Enfile.ApiIntegration;

namespace Enfile
{
    public class FMContentBlock
    {
        public string ExternalCodeId { get; set;}

        public int Priority { get; set; }

        private string Base64Content { get; set;}

        public DateTime CretedAt { get; set; }

        public FMContentBlock() {}

        public FMContentBlock(string externalCodeId)
        {
            this.ExternalCodeId = externalCodeId;
        }

        public FMContentBlock(string externalCodeId, string base64Content)
        {
            this.ExternalCodeId = externalCodeId;
            this.Base64Content = base64Content;
        }

        public byte[] GetData()
        {
            return Convert.FromBase64String(this.Base64Content);
        }

        public FMContentBlock SetData(byte[] data)
        {
            this.Base64Content = Convert.ToBase64String(data);
            return this;
        }

        internal async Task Persist(IFMSession session, Guid fileId)
        {
            var apiS = new FMApiService(session);

            var contentBlock = new ContentBlockApi()
            {
                FileID = fileId,
                ExternalCodeId = this.ExternalCodeId,
                Priority = this.Priority,
                Base64Content = this.Base64Content,
            };

            var block = await apiS.CreateContentBlockAsync(contentBlock);

            this.CretedAt = block.CretedAt;
        }
    }
}