using AspNet.Security.OAuth.Spotify;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.AspNetCore.Builder
{
    public static class SpotifyAuthenticationExtensions
    {
        public static IApplicationBuilder UseSpotifyClientAuthentication(this IApplicationBuilder app)
        {
            // Map the login path to challenge the user with their Spotify Credentials
            app.Map(
                "/login",
                builder =>
                {
                    builder.Run(
                        async context =>
                        {
                            await context.ChallengeAsync(
                                SpotifyAuthenticationDefaults.AuthenticationScheme,
                                properties: new AuthenticationProperties()
                                {
                                    RedirectUri = "/",
                                    IsPersistent = true
                                }
                            );
                        }
                    );
                }
            );
            
            // Map the logout path to sign the user out.
            app.Map(
                "/logout",
                builder =>
                {
                    builder.Run(
                        async context =>
                        {
                            await context.SignOutAsync();
                            context.Response.Redirect("/");
                        }
                    );
                }
            );

            return app;
        }
    }
}