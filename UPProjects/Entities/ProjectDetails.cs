using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UPProjects.Entities
{
    public class ProjectDetails
    {
        public string ProjectId { get; set; }
        public string ProjectTitle { get; set; }
        public string ImageId { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        
        public string ProjectStatus { get; set; }

        public string ProjectCategory { get; set; }

        public string status { get; set; }

        public string UnitName { get; set; }
        public string ZoneName { get; set; }

        public string ZoneId { get; set; }
        public string UnitId { get; set; }
        public string ApprovalStatus { get; set; }

        public string isApproved { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedTime { get; set; }
    }
}
