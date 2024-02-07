using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UPProjects.Entities;

namespace UPProjects.Models
{
    public class AuthenticateResponse
    {

        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
       
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
        public string JwtToken { get; set; }

       // [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }

        public AuthenticateResponse(User user, string jwtToken, string refreshToken)
        {
            Id = user.Id;
            UserId = user.UserId;
            UserName = user.UserName;           
            RoleId = user.RoleId;
            EmployeeId = user.EmployeeId;
            MobileNo = user.MobileNo;
            Email = user.Email;
            OfficeType = user.OfficeType;
            OfficeId = user.OfficeId;
            LastLoginDateTime = user.LastLoginDateTime;
            LastLoginIPAddress = user.LastLoginIPAddress;
            CurrentIPAddress = user.CurrentIPAddress;
            CurrentLoginDateTime = user.CurrentLoginDateTime;
            Role = user.Role;
            UnitId = user.UnitId;
            ZoneId = user.ZoneId;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}
