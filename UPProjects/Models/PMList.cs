using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UPProjects.Models
{
    public class PMList
    {
        public SelectList ZoneList { get; set; }
        public SelectList DistrictList { get; set; }
    }
}
