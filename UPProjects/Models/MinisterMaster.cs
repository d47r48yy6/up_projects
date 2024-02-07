using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace UPProjects.Models
{
    public class MinisterMaster
    {

        public int Id { get; set; }

        [DisplayName("Name of Hon'ble Minister (in English)")]
        public string MainTitle { get; set; }

        [DisplayName("Other Description(in English)")]
        public string SubTitle { get; set; }

        [DisplayName("Name of Hon'ble Minister (in Hindi)")]
        public string HNMainTitle { get; set; }

        [DisplayName("Other Description (in Hindi)")]
        public string HNSubTitle { get; set; }

        public string FileName { get; set; }
        public string Category { get; set; }


        public IFormFile file { get; set; }
    }
}
