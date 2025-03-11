using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiTrading.Service
{
    public class RegistrationService : IApiService<bool>
    {
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;

        public RegistrationService(HttpClient httpClient, AuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        public async Task<bool> FetchDataAsync<Tparam>(Tparam newUser)
        {
            var json = JsonSerializer.Serialize(newUser);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var respons = await _httpClient.PostAsync("https://localhost:7247/api/users/register", content);
                
                if (respons == null)
                    throw new Exception("Could not reach server, try again later.");

                if (respons.IsSuccessStatusCode)
                {
                    await Shell.Current.DisplayAlert("Success", "User registered", "OK");
                    return true;
                }
                else
                {
                    if (respons.StatusCode == System.Net.HttpStatusCode.Conflict)
                        throw new Exception("Username already taken.");
                    else
                        throw new Exception("Failed to register");
                }
            }
            catch(Exception ex)
            {
                await Shell.Current.DisplayAlert("Network Error", ex.Message, "OK");
                return false;
            }

        }
    }
}
