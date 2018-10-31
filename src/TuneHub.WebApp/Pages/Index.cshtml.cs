using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace TuneHub.WebApp.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IHttpContextAccessor _context;

        public IndexModel(IHttpContextAccessor context)
        {
            _context = context;
        }

        public void OnGet()
        {
            ApiKey = _context.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "access_token")?.Value;
        }

        [BindProperty]
        public string ApiKey { get; set; }
    }
}