using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GreenShop.Service
{
    public class JwtService(string secretKey, string issuer, string audience)
    {
        private readonly string _secretKey = secretKey;
        private readonly string _issuer = issuer;
        private readonly string _audience = audience;

        public string GenerateToken(string email, int expireMinutes = 30)
        {
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                _issuer,
                _audience,
                [
                    new Claim(ClaimTypes.Email, email)
                ],
                expires: DateTime.Now.AddMinutes(expireMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateTokenLogin(List<Claim> claim, int expireDays = 30)
        {
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                _issuer,
                _audience,
                claim,
                expires: DateTime.Now.AddDays(expireDays),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal? GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                if (tokenHandler.ReadToken(token) is not JwtSecurityToken)
                    return null;

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,

                    ValidateAudience = true,
                    ValidAudience = _audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

                return principal;
            }
            catch
            {
                return null;
            }
        }


        public static bool IsTokenExpired(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            if (tokenHandler.ReadToken(token) is not JwtSecurityToken jwtToken)
                return true; // Invalid token, consider it expired

            var expiryTimeUnix = long.Parse(jwtToken.Claims.First(claim => claim.Type == "exp").Value);
            Console.WriteLine(expiryTimeUnix);
            var expiryTime = DateTimeOffset.FromUnixTimeSeconds(expiryTimeUnix).DateTime;

            return expiryTime <= DateTime.UtcNow;
        }
    }
}
