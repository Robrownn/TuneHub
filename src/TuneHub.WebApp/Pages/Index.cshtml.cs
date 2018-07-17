using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentSpotifyApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TuneHub.WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IFluentSpotifyClient _fluentSpotifyClient;

        public IndexModel(IFluentSpotifyClient fluentSpotifyClient)
        {
            _fluentSpotifyClient = fluentSpotifyClient;
        }

        public void OnGet()
        {

        }
    }
}
