using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;

namespace UPProjects.Models
{
    public class ServiceMaster
    {
        public int Id { get; set; }
        [DisplayName("Title (in English)")]
        [StringLength(100, ErrorMessage = "Please enter Description.", MinimumLength = 0)]
        public string Title { get; set; }

        [DisplayName("Title (in Hindi)")]
        [StringLength(100, ErrorMessage = "Please enter Description.", MinimumLength = 0)]
        public string HNTitle { get; set; }
        public string FileName { get; set; }
        public IFormFile file { get; set; }
    }
}
