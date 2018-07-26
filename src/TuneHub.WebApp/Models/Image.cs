using Newtonsoft.Json;

namespace TuneHub.WebApp.Models
{
    public class Image
    {
        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("url")]
        public string SourceUrl { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }
    }
}