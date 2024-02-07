using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace UPProjects.Models
{
    public class BoardOfDirectors
    {
        public int ID  { get; set; }
        public int PriorityOrder  { get; set; }
        public string DirectorName  { get; set; }
        public string Designation  { get; set; }
        public string DirectorAddress  { get; set; }
        [DisplayName("Sub Title (in Hindi)")]
        public string HNDirectorName  { get; set; }
        public string HNDesignation { get; set; }
        public string HNDirectorAddress  { get; set; }
        public string CreateBy  { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedIp  { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdatedOn { get; set; }

    }
}
