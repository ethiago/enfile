using System;
using System.ComponentModel.DataAnnotations;

namespace Enfile.Commons.ApiContract.v1
{
    public class ContentBlockApi
    {
        [Required]
        public Guid FileID { get; set; }

        [Required]
        public string ExternalCodeId { get; set;}

        [Required]
        public int Priority { get; set; }

        [Required]
        public string Base64Content { get; set;}

        public DateTime CretedAt { get; set; }
    }
}