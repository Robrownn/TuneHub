using System.Threading.Tasks;
using TuneHub.WebApp.Models;

namespace TuneHub.WebApp.Services.Interfaces
{
    public interface ISpotifyUserService
    {
        Task<SpotifyUser> GetUserProfileAsync();
    }
}