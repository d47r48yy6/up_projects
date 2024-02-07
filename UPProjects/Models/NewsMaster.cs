using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace UPProjects.Models
{
    public class NewsMaster
    {
        public int Id { get; set; }
        [DisplayName("News (in English)")]
        public string NewsTitle { get; set; }
        [DisplayName("News (in Hindi)")]
        public string HNNewsTitle { get; set; }
        [DisplayName("News Content")]
        public string NewsContent { get; set; }
        public string HNNewsContent { get; set; }
        public string ContentType { get; set; }
        public string UserId { get; set; }
        public string UserIP { get; set; }
        public IFormFile file { get; set; }
        public string FileType { get; set; }
        public string  FileSize { get; set; }

        public string Attachment { get; set; }
    }
}
