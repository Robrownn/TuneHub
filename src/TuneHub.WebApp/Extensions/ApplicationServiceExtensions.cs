using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Spotify;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TuneHub.WebApp.Services;
using TuneHub.WebApp.Services.Clients;
using TuneHub.WebApp.Services.Interfaces;

namespace Microsoft.Extensions.DependencyInjection
{
    
    public static class ApplicationServiceExtensions
    {

        /// <summary>
        /// Adds all the services specific to TuneHub.
        /// </summary>
        public static IServiceCollection AddTuneHubServices(this IServiceCollection services)
        {
            services.AddHttpClient<SpotifyClient>(options =>
            {
                options.BaseAddress =  new Uri("https://api.spotify.com/v1/");
            });
            services.AddScoped<ISpotifyPlaylistService, SpotifyPlaylistService>();
            services.AddSignalR();
            return services;
        }

        public static IServiceCollection AddTuneHubAuthentication(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(
                    options =>
                    {
                        options.LoginPath = new PathString("/Login");
                        options.LogoutPath = new PathString("/Logout");
                    }
                )
                .AddSpotify(
                    SpotifyAuthenticationDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.ClientId = Configuration["Authorization:Spotify:ClientId"];
                        options.ClientSecret = Configuration["Authorization:Spotify:ClientSecret"];
                        options.CallbackPath = new PathString(SpotifyAuthenticationDefaults.CallbackPath);
                        options.Scope.Add("playlist-modify-private");
                        options.Scope.Add("playlist-read-private");
                        options.Scope.Add("playlist-read-collaborative");
                        options.Scope.Add("playlist-modify-public");
                        options.Scope.Add("streaming");
                        options.SaveTokens = true;
                        options.Events = new OAuthEvents
                        {
                            OnTicketReceived = context =>
                            {
                                var claimsIdentity = (ClaimsIdentity) context.Principal.Identity;

                                // Add access token to user claims
                                claimsIdentity.AddClaim(new Claim("access_token",
                                    context.Properties.Items.FirstOrDefault(p => p.Key == ".Token.access_token").Value));

                                // Add refresh token to user claims
                                claimsIdentity.AddClaim(new Claim("refresh_token",
                                    context.Properties.Items.FirstOrDefault(p => p.Key == ".Token.refresh_token").Value));

                                return Task.CompletedTask;
                            }
                        };
                    }
                );

            return services;
        }
    }
}