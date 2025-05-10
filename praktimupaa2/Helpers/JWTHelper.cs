using Microsoft.IdentityModel.Tokens;
using praktimupaa2.Models.Login;
using praktimupaa2.Models.Person;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace praktimupaa2.Helpers
{
    public class JwtHelper
    {

        private readonly IConfiguration _configuration;

        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(Auth auth)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, auth.email),
        new Claim(ClaimTypes.Role, auth.password)   
    };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
