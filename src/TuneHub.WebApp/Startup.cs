using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentSpotifyApi.AuthorizationFlows.AspNetCore.AuthorizationCode.Extensions;
using FluentSpotifyApi.AuthorizationFlows.AspNetCore.AuthorizationCode.Handler;
using FluentSpotifyApi.Extensions;
using TuneHub.WebApp.Hubs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace TuneHub.WebApp
{
    public class Startup
    {
        private const string SpotifyAuthenticationScheme = SpotifyDefaults.AuthenticationScheme;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder =>
            {
                builder.AllowAnyMethod().AllowAnyHeader()
                    .WithOrigins("http://localhost:5000")
                    .AllowCredentials();
            }));

            services.AddSignalR();
            services.AddFluentSpotifyClient(
                clientBuilder => clientBuilder.ConfigurePipeline(
                    pipeline => pipeline.AddAspNetCoreAuthorizationCodeFlow(
                        spotifyAuthenticationScheme: SpotifyAuthenticationScheme
                    )));
                
                    
            
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
                
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();

            app.UseSpotifyClientAuthentication();

            app.UseSpotifyInvalidRefreshTokenExceptionHandler("/login");
            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chatHub");
            });

            app.UseMvc();
        }
    }
}
