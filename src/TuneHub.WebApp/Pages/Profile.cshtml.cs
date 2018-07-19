using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TuneHub.WebApp.Models;

namespace TuneHub.WebApp.Pages
{
    public class ProfileModel : PageModel
    {

        public ProfileModel()
        {
        }

        [BindProperty]
        public List<Playlist> Playlists { get; set; }

        [Authorize]
        public void OnGet()
        {

        }
    }
}