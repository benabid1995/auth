using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth
{
    public class JwtAuthenticationManager
    {
        private readonly string key;
        private readonly IDictionary<string, string> users = new Dictionary<string, string>()
        { {"test","password"},{"test1","pwd"}};
        private object securityTokenDescription;
        private object tokenHandler;
        private SecurityTokenDescriptor tokenDescriptor;

        public JwtAuthenticationManager(string key)
        {
            this.key = key;
        }
        public string Authenticate(string username, string password)
        {
            if (!users.Any(u => u.Key == username && u.Value == password))
            { return null; }
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var tokenkey = Encoding.ASCII.GetBytes(key);
            SecurityTokenDescriptor TokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,username)
                }),

                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenkey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(TokenDescriptor);
                

            return tokenHandler.WriteToken(token);
        }
    }
}
