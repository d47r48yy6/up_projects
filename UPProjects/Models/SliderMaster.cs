using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.Web.Mvc;

namespace UPProjects.Models
{
    public class SliderMaster
    {
        public int Id { get; set; }

        [DisplayName("Main Title (in English)")]
        public string MainTitle { get; set; }

        [DisplayName("Sub Title (in English)")]
        public string Title { get; set; }

        [DisplayName("Main Title (in Hindi)")]
        public string HNMainTitle { get; set; }

        [DisplayName("Sub Title (in Hindi)")]
        public string HNTitle { get; set; }

        public string FileName { get; set; }
    
        public IFormFile file { get; set; }
        public string OrderPriroity { get; set; }

        public string FileNameButton { get; set; }
    }
}
