using DesafioPonta.Api.Domain.Authentication;
using DesafioPonta.Api.Domain.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DesafioPonta.Api.Infraestructure.Authentication
{
    public class AuthTokenHandler : ITokenGenerator
    {
        private const int _tokenExpirationInDays = 1;
        private const string _securityKey = "DesafioPontaTokenSecurityKey123456789012";
        private const string _emailClaim = "Email";
        private const string _idClaim = "Id";

        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthTokenHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
     
        public dynamic GenerateToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
               new Claim("Email", usuario.Email),
               new Claim("Id", usuario.Id.ToString()),
            };

            var expiresIn = DateTime.Now.AddDays(_tokenExpirationInDays);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityKey));

            var tokenData = new JwtSecurityToken(
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                expires: expiresIn,
                claims: claims
                );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenData);

            return new
            {
                access_token = token,
                expirations = expiresIn
            };
        }

        public string GetUserIdFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_securityKey);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == "Id").Value;

            return userId;
        }
    }
}
