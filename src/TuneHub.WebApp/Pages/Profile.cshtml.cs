using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentSpotifyApi;
using FluentSpotifyApi.AuthorizationFlows.AspNetCore.AuthorizationCode.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TuneHub.WebApp.Models;

namespace TuneHub.WebApp.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly IFluentSpotifyClient _spotifyClient;

        public ProfileModel(IFluentSpotifyClient spotifyClient)
        {
            _spotifyClient = spotifyClient;
        }

        [BindProperty]
        public List<Playlist> Playlists { get; set; }

        [Authorize]
        public async Task OnGet()
        {
            await ListPlaylists();
        }

        
        public async Task ListPlaylists()
        {
            var userId = this.User.GetNameIdentifier();

            var model = (await _spotifyClient.Me.Playlists.GetAsync(limit: 20, offset: 0))
                .Items
                .Select(item => new Playlist()
                {
                    ID = item.Id,
                    Name = item.Name,
                    Owner = item.Owner?.DisplayName ?? item.Owner?.Id,
                    NumberOfTracks = (item.Tracks?.Total).GetValueOrDefault(),
                    IsPublic = item.Public,
                    IsCollaborative = item.Collaborative,
                    IsOwned = item.Owner?.Id == userId
                }).ToList();

            Playlists = model;
        }
    }
}