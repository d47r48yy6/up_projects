using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UPProjects.Models
{
    public class Supervisor
    {
        public string Id { get; set; }
        public string SupervisorName { get; set; }

        public string MobileNumber { get; set; }
        public string ContractorName { get; set; }
        public string TotalDistricts { get; set; }

        public string TotalSites { get; set; }

        public string WorkStartedSites { get; set; }

        public string WorkInProgressSites { get; set; }
        public string WorkCompletedSites { get; set; }
        public string WorkHandoverSites { get; set; }

    }
}
