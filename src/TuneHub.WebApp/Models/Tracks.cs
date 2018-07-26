using Newtonsoft.Json;

namespace TuneHub.WebApp.Models
{
    public class Tracks
    {
        [JsonProperty("href")]
        public string Link { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }
}