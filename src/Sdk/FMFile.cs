using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Enfile.Commons.ApiContract.v1;
using Enfile.Commons.Model;
using Enfile.ApiIntegration;

namespace Enfile
{
    public class FMFile
    {
        public Guid Id { get; }
        public string FileName { get; }
        public DateTime ClearingDate { get; }
        public FileEncoding Encoding { get; }
        public FileState State { get; }

        private List<FMContentBlock> Content { get; set; }
   
        private FMFile(Guid id, string fileName, DateTime clearingDate, FileEncoding encoding, FileState state)
        {
            this.Id = id;
            this.FileName = fileName;
            this.ClearingDate = clearingDate;
            this.Encoding = encoding;
            this.State = state;
            this.Content = new List<FMContentBlock>();
        }

        public async Task SaveFile(IFMSession session)
        {
            if (session == null) {  throw new ArgumentNullException(nameof(session)); }

            var apiS = new FMApiService(session);

            using( Stream output = File.OpenWrite(this.FileName))
            using(Stream input = await apiS.GetFileStreamAsync(this.Id))
            {
                await input.CopyToAsync(output);
            }
        }

        public async Task LoadContent(IFMSession session)
        {

            if (session == null) {  throw new ArgumentNullException(nameof(session)); }

            var apiS = new FMApiService(session);

            Content.Clear();

            var enumerableContent = await apiS.GetFileContentList(this.Id);

            Content.AddRange( enumerableContent.Select( cr => ConvertFromContentResponse(cr) ) );
        }

        public Task WriteBlockAsync(IFMSession session, byte[] data, string externalCodeId = null)
        {
            externalCodeId = externalCodeId ?? Guid.NewGuid().ToString();
            var contentBlock = new FMContentBlock(externalCodeId).SetData(data);

            lock(Content)
            {
                contentBlock.Priority = Content.Count()+1;
                Content.Add(contentBlock);
            }

            return contentBlock.Persist(session, this.Id);
        }

        public Task WriteBlockAsync(IFMSession session, string data, string externalCodeId = null)
        {
            var bytes =  this.Encoding.ConvertToSystemEncoding().GetBytes(data);
            return this.WriteBlockAsync(session, bytes, externalCodeId);
        }

        public static FMFile OpenWrite(IFMSession session, string fileName, DateTime clearingDate, FileEncoding encoding = FileEncoding.UTF8)
        {
            if (session == null) {  throw new ArgumentNullException(nameof(session)); }

            if (fileName == null) {  throw new ArgumentNullException(nameof(fileName)); }

            if (clearingDate == null) {  throw new ArgumentNullException(nameof(clearingDate)); }

            Guid uniqueIdentifier = Guid.NewGuid();

            var apiS = new FMApiService(session);

            var fileRe = new FileRequest()
            {
                UniqueIdentifier = uniqueIdentifier,
                ClearingDate = clearingDate,
                Encoding = Enum.GetName(typeof(FileEncoding), encoding),
                FileName = fileName
            };

            var fr = apiS.CreateFileAsync(fileRe).Result;

            return ConvertFromFileResponse(fr);
        }

        public static IEnumerable<FMFile> GetFileInfo(IFMSession session, string fileName)
        {
            if (session == null) {  throw new ArgumentNullException(nameof(session)); }

            if (fileName == null) {  throw new ArgumentNullException(nameof(fileName)); }

            var apiS = new FMApiService(session);

            var enumerable = apiS.GetFileInfoAsync(fileName).Result;

            return enumerable.Select( f => ConvertFromFileResponse(f));
        }

        public static FMFile GetFileInfo(IFMSession session, Guid id)
        {
            if (session == null) {  throw new ArgumentNullException(nameof(session)); }

            if (id == null) {  throw new ArgumentNullException(nameof(id)); }

            var apiS = new FMApiService(session);

            var file = apiS.GetFileInfoAsync(id).Result;

            return ConvertFromFileResponse(file);

        }

        private static FMFile ConvertFromFileResponse(FileResponse fr)
        {
            return new FMFile(fr.Id, fr.FileName, fr.ClearingDate, fr.Encoding, fr.State);
        }

        private static FMContentBlock ConvertFromContentResponse(ContentBlockApi cr)
        {
            return new FMContentBlock(cr.ExternalCodeId, cr.Base64Content)
            {
                ExternalCodeId = cr.ExternalCodeId,
                Priority = cr.Priority,
                CretedAt = cr.CretedAt,
            };
        }
    }
}