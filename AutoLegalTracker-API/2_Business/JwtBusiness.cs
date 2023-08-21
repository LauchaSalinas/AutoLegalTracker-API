using AutoLegalTracker_API.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Google.Apis.Oauth2.v2.Data;

namespace AutoLegalTracker_API.Business
{
    public class JwtBusiness
    {
        private IConfiguration _configuration;
        public JwtBusiness(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateJwt(User user)
        {
            // Create the claims for the JWT token
            var claims = GetClaims(user);

            // Create the key and the signing credentials for the JWT token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT_KEY"] ?? String.Empty));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the JWT token
            var jwtToken = new JwtSecurityToken(
                    issuer: _configuration["JWT_ISSUER"] ?? String.Empty,
                    audience: _configuration["JWT_AUDIENCE"] ?? String.Empty,
                    claims: claims,
                    signingCredentials: signIn
                );
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        private Claim[] GetClaims(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Sub),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.Name),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.FamilyName),
                new Claim("ImageUrl", user.GoogleProfilePicture),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(30)).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim("Environment", _configuration["ASPNETCORE_ENVIRONMENT"] ?? string.Empty)
            };

            return claims;
        }
    }

}
