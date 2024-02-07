using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPProjects.Entities;
using UPProjects.Models.AccountViewModel;
using UPProjects.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Dynamic;
using Microsoft.Extensions.Configuration;
using UPProjects.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Protocols;
using System.Text;
using System.Security.Claims;

namespace UPProjects.Controllers
{
    [Authorize(AuthenticationSchemes = AuthSchemes)]
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class TokenAPIController : ControllerBase
    {
        private const string AuthSchemes =
          CookieAuthenticationDefaults.AuthenticationScheme + "," +
          JwtBearerDefaults.AuthenticationScheme;
        private IUserTokenServices _userService;
        private readonly string _connectionString;

        public TokenAPIController(IConfiguration configuration)
        {
           
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Authenticate(ExpandoObject expando)
        {
            PMLogin model = new PMLogin();
            var expandoDict = expando as IDictionary<string, object>;
            model.MobileNumber = expandoDict["MobileNumber"].ToString();
            var response = Authenticate(model, ipAddress());
            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            //setTokenCookie(response.RefreshToken);
            return Ok(response);
        }
        [AllowAnonymous]
        [HttpPost("refreshtoken")]
        public IActionResult RefreshToken([FromBody] RevokeTokenRequest model)
        {

            var response = _userService.RefreshToken(model.Token, ipAddress());
            if (response == null)
                return Unauthorized(new { message = "Invalid token" });

            return Ok(response);
        }

        [HttpPost("revoketoken")]
        public IActionResult RevokeToken([FromBody] RevokeTokenRequest model)
        {

            var token = model.Token;

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });
            var response = _userService.RevokeToken(token, ipAddress());
            if (!response)
                return NotFound(new { message = "Token not found" });
            return Ok(new { message = "Token revoked" });
        }
       // [AllowAnonymous]
        
        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        public AuthenticateResponseNew Authenticate(PMLogin model, string ipAddress)
        {
            var user = (dynamic)null;
            var num = model.MobileNumber;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                user = connection.QueryFirst<PMLogin>("App_CheckPMCredential", new { MobileNumber = model.MobileNumber }, commandType: CommandType.StoredProcedure);
                connection.Close();
            }

            if (user == null) return null;
            if (user != null)
            {
                user.MobileNumber = num;
                var jwtToken = generateJwtToken(user);
                var refreshToken = generateRefreshToken(ipAddress);


                user.RefreshTokens = refreshToken;

                if (insertRefreshTokens(refreshToken, model.MobileNumber))
                    return new AuthenticateResponseNew(user, jwtToken, refreshToken.Token);
                else
                    return null;
            }
            else
                return null;
        }

        public AuthenticateResponse RefreshToken(string token, string ipAddress)
        {
            var newRefreshToken = generateRefreshToken(ipAddress);
            var chkResult = (dynamic)null;
            chkResult = checkExistingRefreshToken(newRefreshToken, token);
            if (chkResult != null)
            {
                if (chkResult.Userid != null && chkResult.Userid != "")
                {
                    var user = (dynamic)null;
                    using (var connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();
                        user = connection.QueryFirst<User>("App_CheckPMCredential", new { UserId = chkResult.Userid }, commandType: CommandType.StoredProcedure);
                        connection.Close();
                    }
                    var jwtToken = generateJwtToken(user);
                    return new AuthenticateResponse(user, jwtToken, newRefreshToken.Token);
                }
                else
                    return null;
            }
            else
                return null;

        }

        public bool RevokeToken(string token, string ipAddress)
        {
            if (revokeExistingRefreshToken(token, ipAddress))
            {
                return true;
            }
            else return false;

        }


        private string generateJwtToken(PMLogin user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            string Secretkey = PTPL_eMandi.Models.ConfigurationManager.AppSetting["AppSettings:Secret"];
            var key = Encoding.ASCII.GetBytes(Secretkey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.PMId.ToString()),
                    new Claim(ClaimTypes.Role, user.Name.ToString()),
                    new Claim(ClaimTypes.Email, user.MobileNumber.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken generateRefreshToken(string ipAddress)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(30),
                    Created = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };
            }
        }
        public bool insertRefreshTokens(RefreshToken refreshToken, string Userid)
        {
            int result = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                result = connection.Execute("InsertRefreshTokens", new { Token = refreshToken.Token, Expires = refreshToken.Expires, CreatedByIp = refreshToken.CreatedByIp, Userid = Userid }, commandType: CommandType.StoredProcedure);
                connection.Close();
            }
            if (result > 0)
                return true;
            else
                return false;
        }
        public dynamic checkExistingRefreshToken(RefreshToken refreshToken, string oldRefreshToken)
        {
            var result = (dynamic)null;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                result = connection.QueryFirstOrDefault("checkExistingRefreshToken", new { oldRefreshToken = oldRefreshToken, NewRefreshToken = refreshToken.Token, Expires = refreshToken.Expires, RevokedByIp = refreshToken.CreatedByIp }, commandType: CommandType.StoredProcedure);
                connection.Close();
            }
            return result;
        }
        public bool revokeExistingRefreshToken(string RefreshToken, string IpAddress)
        {
            int result = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                result = connection.Execute("revokeExistingRefreshToken", new { RefreshToken = RefreshToken, RevokedByIp = IpAddress }, commandType: CommandType.StoredProcedure);
                connection.Close();
            }
            if (result > 0)
                return true;
            else
                return false;
        }


    }
}
