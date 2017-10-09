using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enfile.Core.Model;

namespace Enfile.Core.Interfaces
{
    public interface IFileRepository
    {
        Task<File> Get(Guid id);

        Task<IEnumerable<File>> Get(string fileName);

        Task<IEnumerable<File>> ListOpened();

        Task<IEnumerable<File>> List(DateTime begin, DateTime end);

        File Save(File file);

        Task<IEnumerable<ContentBlock>> GetContent(Guid fileId);
        
        ContentBlock AddContent(ContentBlock contentBlock);
        
        Task<IEnumerable<ContentBlock>> GetOrderedContent(Guid fileId);
    }
}