using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TuneHub.WebApp.Pages
{
    public class LoginModel : PageModel
    {
        public void OnGet()
        {
            
        }

        public void GetClientCredentialsAuthToken()
        {
            var spotifyClient = "";
            var spotifySecret = "";

            var webClient = new WebClient();

            var postParams = new NameValueCollection();
            postParams.Add("grant_type", "client_credentials");

            var authHeader = Convert.ToBase64String(Encoding.Default.GetBytes($"{spotifyClient}:{spotifySecret}"));
            webClient.Headers.Add(HttpRequestHeader.Authorization, "Basic " + authHeader);

            var tokenResponse = webClient.UploadValues("https://accounts.Spotify.com/api/token", postParams);

            var textResponse = Encoding.UTF8.GetString(tokenResponse);
        }
    }
}