using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UPProjects.Models
{
    public class SurveryList
    {

        public string SurveyorId { get; set; }
        public string SurveyDate { get; set; }
        public string SurveyorName { get; set; }
        public string SiteType { get; set; }
        public string Electrity { get; set; }
        public string Water { get; set; }
        public string Districtname { get; set; }
        public string TehsilName { get; set; }
        public string BlockName { get; set; }
        public string Gramsabha { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string FullAddress { get; set; }
        public string CreatedOn { get; set; }

        public string Whethertapwater { get; set; }

        public string Whetherredmark { get; set; }

        public string SurveyStatus { get; set; }
    }
}
