using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UPProjects.Models
{
    public class CMAnnouncement
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public IFormFile file { get; set; }
    }
}
