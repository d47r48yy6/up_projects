using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace UPProjects.Models
{
    public class EOI
    {
        public int Id { get; set; }

        [DisplayName("Title (in English)")]
        public string Title { get; set; }

        [DisplayName("Title (in Hindi)")]

        public string HNTitle { get; set; }

        [DisplayName("GO'S Date")]
        public string GOSDate { get; set; }
        public string FileName { get; set; }

        public IFormFile file { get; set; }

        public string FileSize { get; set; }
        public string FileType { get; set; }
    }
}
