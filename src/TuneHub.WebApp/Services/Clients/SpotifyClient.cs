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
            var accessToken = context.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "access_token")?.Value;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        public async Task<string> GetUserPlaylistsAsync()
        {
            if (IsTokenExpired())
            {
                var isUpdated = await UpdateAccessTokenAsync();
                if (!isUpdated)
                    return null;
            }
                
            return await _client.GetStringAsync("me/playlists");
        }

        public async Task<string> GetUserProfileAsync()
        {
            if (IsTokenExpired())
            {
                var isUpdated = await UpdateAccessTokenAsync();
                if (!isUpdated)
                    return null;
            }

            return await _client.GetStringAsync("me");
        }

        #region Helpers
        private bool UpdateAuthClaims(RefreshToken refreshedToken)
        {
            var claimsIdentity = (ClaimsIdentity) _context.HttpContext.User.Identity;
            try
            {
                claimsIdentity.RemoveClaim(claimsIdentity.FindFirst("access_token"));
                claimsIdentity.AddClaim(new Claim("access_token", refreshedToken.AccessToken));

                claimsIdentity.RemoveClaim(claimsIdentity.FindFirst("expires_at"));
                claimsIdentity.AddClaim(new Claim("expires_at", DateTime.Now.AddSeconds(refreshedToken.ExpiresIn).ToString()));

                return true;
            }
            catch (System.Exception)
            {            
                return false;
            }
            
        }

        private bool IsTokenExpired()
        {
            var expiryDate = Convert.ToDateTime(
                _context.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "expires_at")?.Value
            ).AddMinutes(-1);

            return (DateTime.Now > expiryDate);
        }

        private async Task<bool> UpdateAccessTokenAsync()
        {
            var refreshedToken = JsonConvert.DeserializeObject<RefreshToken>(await _authClient.GetRefreshedAccessTokenAsync());
            var success = UpdateAuthClaims(refreshedToken);

            if (success) 
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", refreshedToken.AccessToken);
                return true;
            }

            return false;
        }
        #endregion

    }
}