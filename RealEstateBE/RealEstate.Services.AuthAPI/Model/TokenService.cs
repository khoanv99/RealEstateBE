namespace RealEstate.Services.AuthAPI.Model
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    public class TokenService
    {
        public string GetUsernameFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
            {
                throw new Exception("Invalid token");
            }

            // Find the "sub" claim to get the username
            var username = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;

            if (string.IsNullOrEmpty(username))
            {
                throw new Exception("Username claim not found in the token");
            }

            return username;
        }
    }
}
