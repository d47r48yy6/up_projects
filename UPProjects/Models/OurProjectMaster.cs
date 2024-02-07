using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.AspNetCore.Http;

namespace UPProjects.Models
{
    public class OurProjectMaster
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string HNTitle { get; set; }
        public string FileName { get; set; }
        public IFormFile file { get; set; }
    }
}
