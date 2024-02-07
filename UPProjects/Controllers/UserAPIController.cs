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
// using Microsoft.AspNetCore.Authentication.JwtBearer;
namespace UPProjects.Controllers
{
   
    [Authorize(AuthenticationSchemes = AuthSchemes)]   
    [Route("api/[controller]")]
    [ApiController]
    public class UserAPIController : ControllerBase
    {
        private const string AuthSchemes =
        CookieAuthenticationDefaults.AuthenticationScheme + "," +
        JwtBearerDefaults.AuthenticationScheme;
        private IUserServices _userService;

        public UserAPIController(IUserServices userService)
        {
            _userService = userService;
        }
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] LoginViewModel model)
        {
            var response = _userService.Authenticate(model, ipAddress());
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
           
             var token = model.Token ;
           
            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });
            var response = _userService.RevokeToken(token, ipAddress());
            if (!response)
                return NotFound(new { message = "Token not found" });
            return Ok(new { message = "Token revoked" });
        }
        [AllowAnonymous]
        [HttpGet("test")]
        public IActionResult test()
        {
            return Ok();
        }
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
    }
}
