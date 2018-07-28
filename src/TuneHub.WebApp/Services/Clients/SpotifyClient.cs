using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TuneHub.WebApp.Models;

namespace TuneHub.WebApp.Services.Clients
{
    public class SpotifyClient
    {
        private readonly SpotifyAuthenticationClient _authClient;
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _context;
        private readonly IConfiguration _config;

        public SpotifyClient(HttpClient client, IHttpContextAccessor context, 
                             IConfiguration config, SpotifyAuthenticationClient authClient)
        {
            _authClient = authClient;
            _client = client;
            _context = context;
            _config = config;

            // Grab the access token and use it as the default request header for authentication
            // Thanks to Volvox.Helios for this: https://github.com/BillChirico/Volvox.Helios/blob/afbee5a9216e3a531a9fe3fbb85ca1f70a0710c3/src/Volvox.Helios.Service/Clients/DiscordAPIClient.cs#L24-L25
            if (IsTokenExpired())
                Task.Run(() => this.UpdateAccessToken()).Wait();
            
            var accessToken = context.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "access_token")?.Value;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        public async Task<string> GetUserPlaylistsAsync()
        {
            return await _client.GetStringAsync("me/playlists");
        }

        public async Task<string> GetUserProfileAsync()
        {
            return await _client.GetStringAsync("me");
        }

        #region Helpers
        private void UpdateAuthClaims(RefreshToken refreshedToken)
        {
            var claimsIdentity = (ClaimsIdentity) _context.HttpContext.User.Identity;

            claimsIdentity.RemoveClaim(claimsIdentity.FindFirst("access_token"));
            claimsIdentity.AddClaim(new Claim("access_token", refreshedToken.AccessToken));

            claimsIdentity.RemoveClaim(claimsIdentity.FindFirst("expires_at"));
            claimsIdentity.AddClaim(new Claim("expires_at", DateTime.Now.AddSeconds(refreshedToken.ExpiresIn).ToString()));
            
        }

        private bool IsTokenExpired()
        {
            var expiryDate = Convert.ToDateTime(
                _context.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "expires_at")?.Value
            ).AddMinutes(-1);

            return (DateTime.Now > expiryDate);
        }

        private async Task UpdateAccessToken()
        {
            var refreshedToken = JsonConvert.DeserializeObject<RefreshToken>(await _authClient.GetRefreshedAccessTokenAsync());
            UpdateAuthClaims(refreshedToken);
        }
        #endregion

    }
}