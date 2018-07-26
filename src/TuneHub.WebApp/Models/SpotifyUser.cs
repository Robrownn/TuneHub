using System.Collections.Generic;
using Newtonsoft.Json;

namespace TuneHub.WebApp.Models
{
    public class SpotifyUser
    {
        [JsonProperty("display_name")]
        public string Name { get; set; }

        [JsonProperty("followers")]
        public SpotifyFollowers Followers { get; set; }

        [JsonProperty("href")]
        public string Link { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("images")]
        public IEnumerable<Image> Images { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("uri")]
        public string SpotifyUri { get; set; }
    }
}