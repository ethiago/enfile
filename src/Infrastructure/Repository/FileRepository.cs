using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cassandra;
using Cassandra.Data.Linq;
using Cassandra.Mapping;
using Enfile.Commons.Model;
using Enfile.Core.Interfaces;
using Enfile.Core.Model;

namespace Enfile.Infrastructure.Repository
{
    public class FileRepository : IFileRepository
    {
        private Table<File> _fileTable;
        private Table<ContentBlock> _contentTable;
        public FileRepository(Table<File> fileTable, Table<ContentBlock> contentTable)
        {
            this._fileTable = fileTable;
            this._contentTable = contentTable;
        }

        public Task<File> Get(Guid id)
        {
            return this._fileTable.Where(f => f.Id == id).First().ExecuteAsync(); 
        }

        public Task<IEnumerable<File>> Get(string fileName)
        {
            return this._fileTable.Where(f => f.FileName == fileName).AllowFiltering().ExecuteAsync(); 
        }

        public Task<IEnumerable<File>> ListOpened()
        {
            return this._fileTable.Where(f => f.State == FileState.Opened).AllowFiltering().ExecuteAsync();
        }

        public Task<IEnumerable<File>> List(DateTime beginInclusive, DateTime endInclusive)
        {
            return this._fileTable.Where( f => f.ClearingDate >= beginInclusive && f.ClearingDate <= endInclusive ).AllowFiltering().ExecuteAsync();
        }

        public File Save(File file)
        {
            this._fileTable.Insert(file).Execute();
            return file;
        }

        public Task<IEnumerable<ContentBlock>> GetContent(Guid fileId)
        {
            var cql = this._contentTable.Where( c => c.FileID == fileId);
            return cql.ExecuteAsync();
        }

        public ContentBlock AddContent(ContentBlock contentBlock)
        {
            var a = this._contentTable.Insert(contentBlock).ExecuteAsync();
            return contentBlock;
        }

        public Task<IEnumerable<ContentBlock>> GetOrderedContent(Guid fileId)
        {
            var cql = this._contentTable.Where( c => c.FileID == fileId).OrderBy(c => c.Priority);
            return cql.ExecuteAsync();
        }
    }
}