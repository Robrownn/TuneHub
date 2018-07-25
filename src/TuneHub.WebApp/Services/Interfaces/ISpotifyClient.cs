using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TuneHub.WebApp.Models;

namespace TuneHub.WebApp.Services.Interfaces
{
    public interface ISpotifyClient
    {
        Task<Playlist> CreatePlaylistAsync(string userId, Playlist newPlaylist);
    }
}