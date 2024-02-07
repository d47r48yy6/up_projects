using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UPProjects.Models
{
    public class NamamiGange
    {
        public string Id { get; set; }
        public string Title { get; set; }
        [StringLength(100, ErrorMessage = "Please enter Description.", MinimumLength = 0)]
        [DataType(DataType.Text)]
        [Display(Name = "Description about Photo (Max Size: 100 Character)")]
        public string Description { get; set; }
        public string FileName { get; set; }
        public IFormFile File { get; set; }
    }
}
