using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UPProjects.Entities;
using UPProjects.Models;
using UPProjects.Models.AccountViewModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using PTPL_eMandi.Models;
namespace UPProjects.Services
{
   
        public interface IUserTokenServices
        {
            AuthenticateResponse Authenticate(PMLogin obj, string ipAddress);
            AuthenticateResponse RefreshToken(string token, string ipAddress);
            bool RevokeToken(string token, string ipAddress);

        }
       public class UserServicesUppcl : IUserTokenServices
    {
            private readonly string _connectionString;
            public UserServicesUppcl(IConfiguration configuration)
            {
                _connectionString = configuration.GetConnectionString("DefaultConnection");
            }
            public AuthenticateResponse Authenticate(PMLogin model, string ipAddress)
            {
                var user = (dynamic)null;
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    user = connection.QueryFirst<PMLogin>("App_CheckPMCredential", new { MobileNumber = model.MobileNumber }, commandType: CommandType.StoredProcedure);
                    connection.Close();
                }

                if (user == null) return null;
                if (user != null)
                {

                    var jwtToken = generateJwtToken(user);
                    var refreshToken = generateRefreshToken(ipAddress);


                    user.RefreshTokens = refreshToken;

                    if (insertRefreshTokens(refreshToken, model.MobileNumber))
                        return new AuthenticateResponse(user, jwtToken, refreshToken.Token);
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


            private string generateJwtToken(User user)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                string Secretkey = ConfigurationManager.AppSetting["AppSettings:Secret"];
                var key = Encoding.ASCII.GetBytes(Secretkey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.OfficeId.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim(ClaimTypes.Email, user.UserId.ToString())
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
