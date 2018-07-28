using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TuneHub.WebApp.Services.Clients
{
    public class SpotifyAuthenticationClient
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _context;
        private readonly IConfiguration _config;

        public SpotifyAuthenticationClient(HttpClient client, IHttpContextAccessor context, IConfiguration config)
        {
            _client = client;
            _context = context;
            _config = config;

            var clientId = config["Authorization:Spotify:ClientId"];
            var clientSecret = config["Authorization:Spotify:ClientSecret"];
            var textBytes = System.Text.Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}");
            var authHeader = System.Convert.ToBase64String(textBytes);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
        }

        /// <summary>
        /// Returns a refreshed access token for a user.
        /// </summary>
        public async Task<string> GetRefreshedAccessTokenAsync()
        {
            var refreshToken = GetUserRefreshToken();
            var postData = CreateRefreshTokenRequestMessage(refreshToken);

            using (var content = new FormUrlEncodedContent(postData))
            {
                content.Headers.Clear();
                content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                var response = await _client.PostAsync("token", content);
                return await response.Content.ReadAsStringAsync();
            }         
            
        }

        private List<KeyValuePair<string,string>> CreateRefreshTokenRequestMessage(string refreshToken)
        {
            var postData = new List<KeyValuePair<string,string>>();
            postData.Add(new KeyValuePair<string, string>("grant_type", "refresh_token"));
            postData.Add(new KeyValuePair<string, string>("refresh_token", $"{refreshToken}"));

            return postData;
        }

        private string GetUserRefreshToken()
        {
            return _context.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "refresh_token")?.Value;
        }
    }

    
}