
using System;
using System.Collections.Generic;
using Enfile.Commons.Model;

namespace Enfile.Commons.ApiContract.v1
{
    public class FileResponse 
    {
        public Guid Id { get; set; }

        public string FileName { get; set; }

        public DateTime ClearingDate { get; set; }

        public FileState State { get; set; }

        public FileEncoding Encoding { get; set; }

        public IEnumerable<ContentBlockApi> Content { get; set; }
    }
}