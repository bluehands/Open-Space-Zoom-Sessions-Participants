using System;
using System.Text;
using System.Threading;
using Microsoft.IdentityModel.Tokens;

namespace ZoomApi
{
    public class TokenGenerator
    {
        private readonly string apiKey;
        private readonly string secret;

        public TokenGenerator(string apiKey, string secret)
        {
            this.apiKey = apiKey;
            this.secret = secret;
        }
        public string Generate()
        {
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var now = DateTime.UtcNow;
            var apiSecret = secret;
            byte[] symmetricKey = Encoding.ASCII.GetBytes(apiSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = apiKey,
                Expires = now.AddSeconds(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}