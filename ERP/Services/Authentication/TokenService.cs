using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ERP.Models;
using Microsoft.AspNetCore.Identity;

namespace ERP.Services.Authentication
{
    public class TokenService
    {
        private IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public TokenService(IConfiguration configuration, UserManager<User> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<string> GenerateToken(User User)
        {
            IList<string> roles = await _userManager.GetRolesAsync(User);

            List<Claim> claims = new List<Claim>
            {
                new Claim("username", User.UserName),
                new Claim("id", User.Id),
                new Claim("loginTimestamp", DateTime.UtcNow.ToString())
            };

            foreach(string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var signingCredentials =
                new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken
                (
                expires: DateTime.Now.AddMinutes(10),
                claims: claims,
                signingCredentials: signingCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
