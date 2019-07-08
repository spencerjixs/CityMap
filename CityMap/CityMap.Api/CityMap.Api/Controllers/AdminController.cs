using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CityMap.Api.Interfaces;
using CityMap.Models;
using CityMap.Models.Contracts;
using CityMap.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CityMap.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminManager _adminManager;
        private readonly AppSettings _appSettings;
        public AdminController(IAdminManager adminManager, IOptions<AppSettings> appSettings)
        {
            _adminManager = adminManager;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/Admin/Login")]
        public async Task<IActionResult> Login([FromBody]UserLoginRequest userLoginRequest)
        {
            var user = await _adminManager.UserLogin(userLoginRequest.UserName, userLoginRequest.Password);
            if (user == null || user.UserName != userLoginRequest.UserName)
            {
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.JWTAuthenticationSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName+"|"+ user.Password)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                AtuhToken = tokenString
            });
        }

        [HttpGet]
        [Route("api/Admin/GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _adminManager.GetUsers();
            return Ok(result);
        }

        [HttpPost]
        [Route("api/Admin/AddUser")]
        public async Task<IActionResult> AddUser([FromBody] AdminUser adminUser)
        {
            var result = await _adminManager.AddUser(adminUser);
            return Ok(result);
        }

        [HttpPost]
        [Route("api/Admin/UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] AdminUser adminUser)
        {
            var result = await _adminManager.UpdateUser(adminUser);
            return Ok(result);
        }
    }
}
