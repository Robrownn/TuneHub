using System;
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
            var postData = new JObject
            {
                new JProperty("grant_type", "refresh_token"),
                new JProperty("refresh_token", refreshToken)
            };

            var response = await _client.PostAsJsonAsync("token", postData);
            var responseObject = JsonConvert.DeserializeObject<RefreshToken>(response.Content.ToString());
            return responseObject.AccessToken;
        }

        private string GetUserRefreshToken()
        {
            return _context.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "refresh_token")?.Value;
        }

        struct RefreshToken
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }

            [JsonProperty("token_type")]
            public string TokenType { get; set; }

            [JsonProperty("scope")]
            public string Scope { get; set; }

            [JsonProperty("expires_in")]
            public int ExpiresIn { get; set; }
        }
    }

    
}