using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TuneHub.WebApp.Models
{
    public class Playlist
    {
        [JsonProperty("collaborative")]
        public bool IsCollaborative { get; set; }

        [JsonProperty("href")]
        public string Link { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("images")]
        public IEnumerable<Image> Images { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("owner")]
        public SpotifyUser Owner { get; set; }

        [JsonProperty("public")]
        public bool? IsPublic { get; set; }

        [JsonProperty("snapshot_id")]
        public string SnapshotId { get; set; }

        [JsonProperty("tracks")]
        public Tracks Tracks { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("uri")]
        public string SpotifyUri { get; set; }
    }

    public class Playlists
    {
        [JsonProperty("items")]
        public IEnumerable<Playlist> List { get; set; }
    }
}