using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TuneHub.WebApp.Models;

namespace TuneHub.WebApp.Services.Clients.Interfaces
{
    public interface ISpotifyClient
    {
        Task<string> GetUserPlaylistsAsync();
    }
}