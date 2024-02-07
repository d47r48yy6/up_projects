using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UPProjects.Entities
{
    public class User
    {
        
        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RoleId { get; set; }
        public string EmployeeId { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string OfficeType { get; set; }
        public string OfficeId { get; set; }

        public string LastLoginDateTime { get; set; }
        public string LastLoginIPAddress { get; set; }
        public string CurrentIPAddress { get; set; }
        public string CurrentLoginDateTime { get; set; }
        public string Role { get; set; }     
        public string UnitId { get; set; }
        public string ZoneId { get; set; }

        [JsonIgnore]
        
         public RefreshToken RefreshTokens { get; set; }
    }
}
