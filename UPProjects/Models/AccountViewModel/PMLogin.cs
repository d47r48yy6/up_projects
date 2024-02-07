using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UPProjects.Entities;

namespace UPProjects.Models.AccountViewModel
{
    public class PMLogin
    {
        public string ZoneId { get; set; }
        public string UnitId { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public string ZoneName { get; set; }
        public string UnitName { get; set; }
        public string PMId { get; set; }
        public string MobileNumber { get; set; }
        [JsonIgnore]
        public RefreshToken RefreshTokens { get; set; }

    }
}
