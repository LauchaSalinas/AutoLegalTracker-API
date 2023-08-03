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

        public JwtSecurityToken CreateJwt(Userinfo meObject)
        {
            // Create the claims for the JWT token
            var claims = GetClaims(meObject);

            // Create the key and the signing credentials for the JWT token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT_KEY"] ?? String.Empty));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the JWT token
            var jwtToken = new JwtSecurityToken(
                    issuer: _configuration["JWT_ISSUER"] ?? String.Empty,
                    audience: _configuration["JWT_AUDIENCE"] ?? String.Empty,
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials: signIn
                );

            return jwtToken;
        }

        private Claim[] GetClaims(Userinfo meObject)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, meObject.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, meObject.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, meObject.GivenName),
                new Claim(JwtRegisteredClaimNames.FamilyName, meObject.FamilyName),
                // TODO create custom claims

                new Claim("Environment", _configuration["ASPNETCORE_ENVIRONMENT"] ?? string.Empty)
            };

            return claims;
        }
    }

}
