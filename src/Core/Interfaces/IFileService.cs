using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Enfile.Commons.Model;
using Enfile.Core.Model;
using AppFile = Enfile.Core.Model.File;

namespace Enfile.Core.Interfaces
{
    public interface IFileService
    {
        AppFile CreateFile(Guid id, string fileName, DateTime clearingDate, FileEncoding encoding);
        Task<AppFile> GetFileInfo(Guid id);
        Task<IEnumerable<AppFile>> GetFileInfo(string filename);
        Task<IEnumerable<ContentBlock>> GetFileContent(Guid fileId);
        ContentBlock AddContentToFile(Guid fileId, string externalCodeId, int priority, string base64Content);
        IEnumerable<byte[]> GetFileBytes(AppFile fileId);
    }
}