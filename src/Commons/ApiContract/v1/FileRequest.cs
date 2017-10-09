using System;
using System.ComponentModel.DataAnnotations;
using Enfile.Commons.Model;

namespace Enfile.Commons.ApiContract.v1
{
    public class FileRequest
    {
        [Required]
        public Guid UniqueIdentifier { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9-_][a-zA-Z0-9-_\\.]*$")]
        public string FileName { get; set; }

        [Required]
        public DateTime ClearingDate { get; set; }

        [Required]
        [EnumDataType(typeof(FileEncoding))]
        public string Encoding { get; set; }
    }
}