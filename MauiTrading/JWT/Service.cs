using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;

namespace MauiTrading.JWT
{
    class Service
    {
        private const string TokenKey = "auth_token";

        //Spara token
        public static async Task SaveTokenAsync(string token)
        {
            await SecureStorage.SetAsync(TokenKey, token);
        }

        //Hämta token
        public static async Task<string> GetTokenAsync()
        {
            return await SecureStorage.GetAsync(TokenKey);
        }

        //Radera token
        public static void RemoveToken()
        {
            SecureStorage.Remove(TokenKey);
        }

        //Validerar token
        public static async Task<bool> IsTokenValidAsync()
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
                return false;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            return jwtToken != null && jwtToken.ValidTo > DateTime.UtcNow;
        }

        //httpClient med JWT-token som Authorization-header
        public static async Task<HttpClient> GetAuthenticatedHttpClientAsync()
        {
            var client = new HttpClient();
            var token = await GetTokenAsync();

            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return client;
        }


        public static async Task<string> GetUsernameAsync()
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            var usernameClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);

            return usernameClaim?.Value;
        }
    }
}
