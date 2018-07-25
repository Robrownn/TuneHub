using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TuneHub.WebApp.Models;
using TuneHub.WebApp.Services.Interfaces;

namespace TuneHub.WebApp.Services
{
    public class SpotifyClient : ISpotifyClient
    {
        public const string baseUrl = "https://api.spotify.com/v1/";

        public async Task<Playlist> CreatePlaylistAsync(string userId, Playlist newPlaylist)
        {
            string postData = JsonConvert.SerializeObject(newPlaylist);
            using(var client = new HttpClient())
            {
                try
                {
                    var httpContent = new StringContent(postData, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync($"{baseUrl}users/{userId}/playlists", httpContent);

                    if (response.StatusCode == HttpStatusCode.Created)
                        return newPlaylist;

                    return null;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}