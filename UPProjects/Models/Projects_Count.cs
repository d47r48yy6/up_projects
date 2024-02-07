using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UPProjects.Models
{
    public class Projects_Count
    {
        public int id { get; set; }
        public int CompletedProjects { get; set; }
        public int HandoverProjects { get; set; }
        public int NonHandoverProjects { get; set; }
        public int TotalOngoingProjects { get; set; }
        public int TotalProjects { get; set; }
        public DateTime LastUpdateOn { get; set; }
        public string LastUpdateBy { get; set; }
        public string UpdateIP { get; set; }
        public string RegistrationForm { get; set; }
        public string AnnexureForm { get; set; }
        public string OrganizationImg { get; set; }
        public string TenderForm { get; set; }

    }
}
