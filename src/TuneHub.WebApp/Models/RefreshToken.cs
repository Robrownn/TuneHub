using Newtonsoft.Json;

namespace TuneHub.WebApp.Models
{
    public class RefreshToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("expires_in")]
        public double ExpiresIn { get; set; }
    }
}