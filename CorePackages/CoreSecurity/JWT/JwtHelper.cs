using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CoreSecurity.Claims;
using CoreSecurity.Encryption;
using CoreSecurity.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


namespace CoreSecurity.JWT
{
    public class JwtHelper:ITokenHelper
    {
        private readonly IConfiguration _configuration;
        private readonly TokenOptions _tokenOptions;

        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _tokenOptions = configuration.GetSection("TokenOptions").Get<TokenOptions>()
          ?? throw new InvalidOperationException("TokenOptions configuration is missing.");
        }

        public RefreshToken CreateRefreshToken(User user, string ipAddress)
        {

            RefreshToken refreshToken =
               new()
               {
                   UserId = user.EntityID,
                   Token = RandomRefreshToken(),
                   Expires = DateTime.UtcNow.AddDays(7),
                   CreatedByIp = ipAddress
               };

            return refreshToken;
        }

        public AccessToken CreateToken(User user, IList<OperationClaim> operationClaims)
        {
            var expiresIn = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);

            SecurityKey securityKey = SecurityKeyHelper.SecurityKey(_tokenOptions.SecurityKey);
            SigningCredentials signingCredentials = SigningCredentialsHelper.SigningCredentials(securityKey);
            JwtSecurityToken jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, operationClaims, expiresIn);

            string? token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return new AccessToken { Token = token, Expiration = expiresIn };
        }

        private JwtSecurityToken CreateJwtSecurityToken(
            TokenOptions tokenOptions, User user,
            SigningCredentials signingCredentials,
            IList<OperationClaim> operationClaims,
            DateTime expiresIn)
        {
            return new JwtSecurityToken(
                tokenOptions.Issuer,
                tokenOptions.Audience,
                expires: expiresIn,
                notBefore: DateTime.Now,
                claims: SetClaims(user, operationClaims),
                signingCredentials: signingCredentials
            );
        }
        
       

        private IEnumerable<Claim> SetClaims(User user, IList<OperationClaim> operationClaims)
        {
            List<Claim> claims = new();
            claims.AddNameIdentifier(user.EntityID.ToString());
            claims.AddEMail(user.EMail);
            claims.AddName($"{user.FirstName} {user.LastName}");
            claims.AddRole(operationClaims.Select(c => c.Name).ToArray());
            return claims;
        }

        private string RandomRefreshToken()
        {
            byte[] numberByte = new byte[32];
            using var random = RandomNumberGenerator.Create();
            random.GetBytes(numberByte);
            return Convert.ToBase64String(numberByte);
        }
    }
}
