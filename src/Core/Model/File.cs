
using System;
using System.Collections.Generic;
using Enfile.Commons.Model;

namespace Enfile.Core.Model
{
    public class File
    {
        public File() {}

        private File(Guid id, string fileName, DateTime clearingDate, FileEncoding encoding)
        {
            this.Id = id;
            this.FileName = fileName;
            this.ClearingDate = clearingDate.Date;
            this.State = FileState.Opened;
            this.Encoding = encoding;
        }

        public Guid Id { get; set; }

        public string FileName { get; set; }

        public DateTime ClearingDate { get; set; }

        public FileState State { get; set; }

        public FileEncoding Encoding { get; set; }

        public IEnumerable<ContentBlock> Content { get; set; }

        public static File Create(Guid id, string fileName, DateTime clearingDate, FileEncoding encoding)
        {
            return new File(id, fileName, clearingDate, encoding);
        }
    }
}