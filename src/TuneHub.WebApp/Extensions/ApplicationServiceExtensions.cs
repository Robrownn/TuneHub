using FluentSpotifyApi.AuthorizationFlows.AspNetCore.AuthorizationCode.Extensions;
using FluentSpotifyApi.AuthorizationFlows.AspNetCore.AuthorizationCode.Handler;
using FluentSpotifyApi.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    
    public static class ApplicationServiceExtensions
    {
        private const string SpotifyAuthenticationScheme = SpotifyDefaults.AuthenticationScheme;

        /// <summary>
        /// Adds all the services specific to TuneHub.
        /// </summary>
        public static IServiceCollection AddTuneHubServices(this IServiceCollection services)
        {
            services.AddSignalR();
            services.AddFluentSpotifyClient(
                clientBuilder => clientBuilder.ConfigurePipeline(
                    pipeline => pipeline.AddAspNetCoreAuthorizationCodeFlow(
                        spotifyAuthenticationScheme: SpotifyAuthenticationScheme
                    )));

            return services;
        }

        /// <summary>
        /// Adds the authentication methods and options specific to TuneHub.
        /// </summary>
        /// <param name="Configuration">The configuration file that holds the Spotify ClientID and ClientSecret</param>
        public static IServiceCollection AddTuneHubAuthentication(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(
                    options =>
                    {
                        options.LoginPath = new PathString("/login");
                        options.LogoutPath = new PathString("/logout");
                    })
                .AddSpotify(
                    SpotifyAuthenticationScheme,
                    options =>
                    {
                        options.ClientId = Configuration["Authorization:Spotify:ClientId"];
                        options.ClientSecret = Configuration["Authorization:Spotify:ClientSecret"];
                        options.Scope.Add("playlist-read-private");
                        options.Scope.Add("playlist-read-collaborative");
                        options.SaveTokens = true;
                    });

            return services;
        }
    }
}