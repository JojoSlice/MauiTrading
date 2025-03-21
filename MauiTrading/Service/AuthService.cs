﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiTrading.Service
{
    public class AuthService
    {

        private readonly HttpClient _httpClient;
        private const string TokenKey = "AuthToken";
        public User? CurrentUser { get; private set; }


        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> LoginAsync(LoginDto loginDto)
        {
            var json = JsonSerializer.Serialize(loginDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var respons = await _httpClient.PostAsync("https://localhost:7247/api/users/login", content);

                if (respons == null)
                    throw new Exception("Could not reach server, try again later.");

                if (respons.IsSuccessStatusCode)
                {
                    var responsBody = await respons.Content.ReadAsStringAsync();
                    var responsObject = JsonSerializer.Deserialize<JsonElement>(responsBody);

                    var token = responsObject.GetProperty("token").GetString();

                    await SecureStorage.SetAsync(TokenKey, token);
                    CurrentUser = new User { username = loginDto.username, token = token };

                    await Shell.Current.GoToAsync(nameof(HomePage));
                    return true;
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Invalid username or password", "OK");
                    return false;
                }
            }
            catch
            {
                await Shell.Current.DisplayAlert("Network Error", "Could not reach server", "OK");
                return false;
            }
        }
        public async Task<bool> TryAutoLoginAsync()
        {
            string? token = await SecureStorage.GetAsync(TokenKey);
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

                var usernameClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);

                if (usernameClaim == null)
                {
                    return false;
                }
                else
                {
                    CurrentUser = new User { username = usernameClaim.Value, token = token };
                    return true;
                }

            }
            return false;
        }
        public async Task LogoutAsync()
        {
            try
            {
                SecureStorage.RemoveAll();
                CurrentUser = null;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Error during logout: {ex.Message}", "OK");
                throw;
            }
        }

        

        public class LoginDto
        {
            public string username { get; set; } = string.Empty;
            public string password { get; set; } = string.Empty;
        }
        public class User
        {
            public string username { get; set; } = string.Empty;
            public string token { get; set; } = string.Empty;
        }
    }
}
