using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UPProjects.Models
{
    public class SurveyDetail
    {
        public string Id { get; set; }
        public string DistrictId { get; set; }
        public string TehsilId { get; set; }
        public string BlockId { get; set; }
        public string GramSabhaId { get; set; }
        public string SiteType { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string ElectricityStatus { get; set; }
        public string HandPupWaterStatus { get; set; }
        public string FullAddress { get; set; }
        public string IPAddress { get; set; }
        public string Whethertapwater { get; set; }

        public string Whetherredmark { get; set; }

        public List<AppImage> Image = new List<AppImage>();
    }

    public class AppImage
    {

        public string imag { get; set; }

    }
}
