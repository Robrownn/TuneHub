using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TuneHub.WebApp.Models;
using TuneHub.WebApp.Services.Interfaces;

namespace TuneHub.WebApp.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly ISpotifyPlaylistService _spotifyPlaylists;
        private readonly ISpotifyUserService _spotifyUsers;

        public ProfileModel(ISpotifyPlaylistService spotifyPlaylists,
                            ISpotifyUserService spotifyUsers)
        {
            _spotifyPlaylists = spotifyPlaylists;
            _spotifyUsers = spotifyUsers;
        }

        [BindProperty]
        public Playlists Playlists { get; set; }

        [BindProperty]
        public SpotifyUser Me { get; set; }

        [Authorize]
        public async Task OnGet()
        {
            Playlists = await _spotifyPlaylists.GetPlaylistsAsync();
            Me = await _spotifyUsers.GetUserProfileAsync();
        }
    }
}