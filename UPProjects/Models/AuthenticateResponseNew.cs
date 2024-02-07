using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UPProjects.Models.AccountViewModel;

namespace UPProjects.Models
{
    public class AuthenticateResponseNew
    {
        public string ZoneId { get; set; }
        public string UnitId { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public string ZoneName { get; set; }
        public string UnitName { get; set; }
        public string PMId { get; set; }
        public string MobileNumber { get; set; }
        public string  RefreshTokens { get; set; }
        public string JwtToken { get; set; }
        public AuthenticateResponseNew(PMLogin user, string jwtToken, string refreshToken)
        {
            
            UnitId = user.UnitId;
            ZoneId = user.ZoneId;
            JwtToken = jwtToken;
            PMId = user.PMId;
            Name = user.Name;
            ZoneName = user.ZoneName;
            UnitName = user.UnitName;
            MobileNumber = user.MobileNumber;
            RefreshTokens = refreshToken;
        }
    }
   
}
