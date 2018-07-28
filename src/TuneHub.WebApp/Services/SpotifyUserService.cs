using System.Threading.Tasks;
using Newtonsoft.Json;
using TuneHub.WebApp.Models;
using TuneHub.WebApp.Services.Clients;
using TuneHub.WebApp.Services.Interfaces;

namespace TuneHub.WebApp.Services
{
    public class SpotifyUserService : ISpotifyUserService
    {
        private readonly SpotifyClient _client;

        public SpotifyUserService(SpotifyClient client)
        {
            _client = client;
        }

        public async Task<SpotifyUser> GetUserProfileAsync()
        {
            var user = await _client.GetUserProfileAsync();

            if (user == null)
                return null;

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            return JsonConvert.DeserializeObject<SpotifyUser>(user, settings);
        }
    }
}