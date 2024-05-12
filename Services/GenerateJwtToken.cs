using colab_api.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace colab_api.Services
{
    public class GenerateJwtToken
    {
        public string GenerateToken(Users user) {
            var SKey = Environment.GetEnvironmentVariable("key");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Name, user.username),
                    // เพิ่มข้อมูลเพิ่มเติมลงใน JWT ตามต้องการ
                }),
                Expires = DateTime.UtcNow.AddHours(1), // กำหนดเวลาหมดอายุของ Token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
       
    }
}
