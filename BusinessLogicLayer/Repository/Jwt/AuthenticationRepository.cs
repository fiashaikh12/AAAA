using BusinessObjects.Entities.Jwt;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repository
{
    public class AuthenticationRepository
    {

        private static AuthenticationRepository instance = null;
        private AuthenticationRepository() {
            /// <summary>
            /// Use the below code to generate symmetric Secret Key
            var hmac = new HMACSHA256();
            var key = Convert.ToBase64String(hmac.Key);
            /// </summary>
        }
        public static AuthenticationRepository GetInstance
        {
            get
            {
                if (instance == null) { instance = new AuthenticationRepository(); }
                return instance;
            }
        }
        private const string communicationKey = "GQDstc21ewfffffffffffFiwDffVvVBrk";
        SecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(communicationKey));
        // The Method is used to generate token for user
        public string GenerateToken(string userName,int expireMinutes=20)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(communicationKey));
            var now = DateTime.UtcNow;
            var signingCredentials = new SigningCredentials(signingKey,
               SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);

            var claimsIdentity = new ClaimsIdentity(new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userName)
            }, "Custom");

            var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                
                IssuedAt= DateTime.Now,
                Issuer = "self",
                Subject = claimsIdentity,
                SigningCredentials = signingCredentials,
                Expires = now.AddMinutes(expireMinutes),
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var plainToken = tokenHandler.CreateToken(securityTokenDescriptor);
            var signedAndEncodedToken = tokenHandler.WriteToken(plainToken);

            return signedAndEncodedToken;

        }

        /// Using the same key used for signing token, user payload is generated back
        //public JwtSecurityToken GenerateUserClaimFromJWT(string authToken)
        //{

        //    var tokenValidationParameters = new TokenValidationParameters()
        //    {
        //        ValidAudiences = new string[]
        //              {
        //            "http://www.md.com",
        //              },

        //        ValidIssuers = new string[]
        //          {
        //              "self",
        //          },
        //        IssuerSigningKey = signingKey
        //    };
        //    var tokenHandler = new JwtSecurityTokenHandler();

        //    SecurityToken validatedToken;

        //    try
        //    {

        //        tokenHandler.ValidateToken(authToken, tokenValidationParameters, out validatedToken);
        //    }
        //    catch (Exception)
        //    {
        //        return null;

        //    }

        //    return validatedToken as JwtSecurityToken;

        //}

        //private JWTAuthenticationIdentity PopulateUserIdentity(JwtSecurityToken userPayloadToken)
        //{
        //    string name = ((userPayloadToken)).Claims.FirstOrDefault(m => m.Type == "unique_name").Value;
        //    string userId = ((userPayloadToken)).Claims.FirstOrDefault(m => m.Type == "nameid").Value;
        //    return new JWTAuthenticationIdentity(name) { UserId = Convert.ToInt32(userId), UserName = name };

        //}


        private static bool ValidateToken(string token, out string username)
        {
            username = null;
            var simplePrinciple = GetPrincipal(token);
            var identity = simplePrinciple.Identity as ClaimsIdentity;

            if (identity == null)
                return false;

            if (!identity.IsAuthenticated)
                return false;

            var usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim?.Value;

            if (string.IsNullOrEmpty(username))
                return false;

            // More validate to check whether username exists in system

            return true;
        }
        protected Task<IPrincipal> AuthenticateJwtToken(string token)
        {
            string username;

            if (ValidateToken(token, out username))
            {
                // based on username to get more information from database in order to build local identity
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username)
                // Add more claims if needed: Roles, ...
            };

                var identity = new ClaimsIdentity(claims, "Jwt");
                IPrincipal user = new ClaimsPrincipal(identity);

                return Task.FromResult(user);
            }

            return Task.FromResult<IPrincipal>(null);
        }

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var symmetricKey = Convert.FromBase64String(communicationKey);

                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                return principal;
            }

            catch (Exception)
            {
                //should write log
                return null;
            }
        }
    }

}
