using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TuneHub.WebApp.Models;
using TuneHub.WebApp.Services.Clients.Interfaces;

namespace TuneHub.WebApp.Services.Clients
{
    public class SpotifyClient : ISpotifyClient
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _context;
        private readonly IConfiguration _config;

        public SpotifyClient(HttpClient client, IHttpContextAccessor context, IConfiguration config)
        {
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
            return await _client.GetStringAsync($"users/me/playlists");
        }


    }
}