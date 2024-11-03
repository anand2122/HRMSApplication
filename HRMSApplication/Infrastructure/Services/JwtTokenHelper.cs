using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HRMSApplication.Infrastructure.Services
{
    public static class JwtTokenHelper
    {
        public static string GenerateToken(string username, string key, string issuer, string audience) 
        { 
            var tokenHandler = new JwtSecurityTokenHandler(); 
            var keyBytes = Encoding.ASCII.GetBytes(key); 
            var tokenDescriptor = new SecurityTokenDescriptor 
            { 
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, username) }), 
                    Expires = DateTime.UtcNow.AddHours(1), 
                    Issuer = issuer,
                    Audience = audience, 
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), 
                    SecurityAlgorithms.HmacSha256Signature) 
            };
            var token = tokenHandler.CreateToken(tokenDescriptor); 
            return tokenHandler.WriteToken(token); 
        }
        public static ClaimsPrincipal ValidateToken(string token, string key, string issuer, string audience) 
        {
            var tokenHandler = new JwtSecurityTokenHandler(); 
            var keyBytes = Encoding.ASCII.GetBytes(key); 
            var tokenValidationParameters = new TokenValidationParameters 
            {
                ValidateIssuerSigningKey = true, 
                IssuerSigningKey = new SymmetricSecurityKey(keyBytes), 
                ValidateIssuer = true, 
                ValidateAudience = true, 
                ValidIssuer = issuer, 
                ValidAudience = audience, 
                ClockSkew = TimeSpan.Zero }; 
            try 
            { 
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken); 
                return principal; 
            } catch 
            { 
                return null; 
            } 
        }
    }
}
