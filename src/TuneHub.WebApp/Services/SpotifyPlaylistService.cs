using System.Threading.Tasks;
using Newtonsoft.Json;
using TuneHub.WebApp.Models;
using TuneHub.WebApp.Services.Clients;
using TuneHub.WebApp.Services.Interfaces;

namespace TuneHub.WebApp.Services
{
    public class SpotifyPlaylistService : ISpotifyPlaylistService
    {
        private readonly SpotifyClient _client;

        public SpotifyPlaylistService(SpotifyClient client)
        {
            _client = client;
        }

        public async Task<Playlists> GetPlaylistsAsync()
        {
            var playlists = await _client.GetUserPlaylistsAsync();

            if (playlists == null)
                return null;

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            return JsonConvert.DeserializeObject<Playlists>(playlists, settings);
        }
    }
}