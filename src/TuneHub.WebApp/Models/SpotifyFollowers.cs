using Newtonsoft.Json;

namespace TuneHub.WebApp.Models
{
    public class SpotifyFollowers
    {
        [JsonProperty("href")]
        public string Link { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }
}