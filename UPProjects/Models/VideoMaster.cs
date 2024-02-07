using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace UPProjects.Models
{
    public class VideoMaster
    {
        public int Id { get; set; }

        [DisplayName("Title")]
        public string Title { get; set; }

        public string FileName { get; set; }

        public IFormFile file { get; set; }
    }
}
