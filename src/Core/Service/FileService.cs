using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enfile.Commons.Model;
using Enfile.Core.Interfaces;
using Enfile.Core.Model;
using System.IO;
using AppFile = Enfile.Core.Model.File;

namespace Enfile.Core.Service
{
    public class FileService : IFileService
    {
        private IFileRepository _repository;

        public FileService(IFileRepository repository)
        {
            this._repository = repository;
        }

        public AppFile CreateFile(Guid id, string fileName, DateTime clearingDate, FileEncoding encoding)
        {     
            return this._repository.Save( AppFile.Create(id, fileName, clearingDate, encoding));
        }

        public async Task<AppFile> GetFileInfo(Guid id)
        {
            return await this._repository.Get( id );
        }

        public async Task<IEnumerable<AppFile>> GetFileInfo(string filename)
        {
            return await this._repository.Get( filename );
        }

        public async Task<IEnumerable<ContentBlock>> GetFileContent(Guid fileId)
        {
            return await this._repository.GetContent( fileId );
        }

        public ContentBlock AddContentToFile(Guid fileId, string externalCodeId, int priority, string base64Content)
        {
            return this._repository.AddContent( ContentBlock.Create(fileId, externalCodeId, base64Content, priority, DateTime.Now));
        }

        public IEnumerable<byte[]> GetFileBytes(AppFile file)
        {
            var contentEnumerable = this._repository.GetOrderedContent( file.Id ).Result;

            return contentEnumerable.Select( cont =>  Convert.FromBase64String(cont.Base64Content));
        }
    }
}