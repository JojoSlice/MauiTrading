using MauiTrading.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MauiTrading.Service
{
    public class UserService : IApiService<Models.User>
    {
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;
        public UserService(HttpClient httpClient, AuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }
        
        public async Task<Models.User> FetchDataAsync<TParam>(TParam? param)
        {
            var user = _authService.CurrentUser;
            var response = await _httpClient.GetAsync($"https://localhost:7247/api/users/getuser?username={user.username}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Models.User>() ?? throw new Exception("User not found");
        }
    }
}
