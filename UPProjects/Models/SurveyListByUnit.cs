using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UPProjects.Models
{
    public class SurveyListByUnit
    {
        public int ZoneId { get; set; }
        public int UnitId { get; set; }
        public int DistrictId { get; set; }

        public int School { get; set; }
        public int Anganwadi { get; set; }
        public int Aashram { get; set; }

        public string ZonerName { get; set; }

        public string UnitName { get; set; }
        public string DistrictName { get; set; }

    }
}
