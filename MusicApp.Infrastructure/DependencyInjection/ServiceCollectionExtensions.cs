using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MusicApp.Application.Features.Auth.Services;
using MusicApp.Application.Features.Email.Services;
using MusicApp.Application.Features.Playlist.Repositories;
using MusicApp.Application.Features.Spotify.Services;
using MusicApp.Core.Entities.Auth;
using MusicApp.Infrastructure.Configuration;
using MusicApp.Infrastructure.Persistence;
using MusicApp.Infrastructure.Repositories;
using MusicApp.Infrastructure.Services.Auth;
using MusicApp.Infrastructure.Services.Email;
using MusicApp.Infrastructure.Services.Spotify;
using System.Text;

namespace MusicApp.Infrastructure.DependencyInjection
{
    /// <summary>
    /// Extension methods to register Infrastructure services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers Infrastructure services: DbContext, Identity, JWT, Email sender, and settings.
        /// </summary>
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Bind configuration sections
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
            services.Configure<SpotifySettings>(configuration.GetSection("SpotifySettings"));

            // 1) DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // 2) Identity
            services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Lockout.MaxFailedAccessAttempts = 5;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            // 3) JWT Authentication
            var jwtSecret = configuration.GetValue<string>("JwtSettings:Secret")
                            ?? throw new InvalidOperationException("JWT Secret not configured");
            var key = Encoding.ASCII.GetBytes(jwtSecret);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

            // 4) Infrastructure services and repositories
            services.AddTransient<IEmailSenderService, EmailSenderService>();
            services.AddSingleton<IJwtService, JwtService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IEmailTemplateService, EmailTemplateService>();
            services.AddScoped<IImageEmbedService, ImageEmbedService>();
            services.AddScoped<IPlaylistRepository, PlaylistRepository>();
            services.AddScoped<IPlaylistTrackRepository, PlaylistTrackRepository>();
            services.AddScoped<ITrackRepository, TrackRepository>();
            services.AddScoped<IPlaylistShareRepository, PlaylistShareRepository>();


            // 5) Spotify
            services.AddHttpClient<ISpotifyService, SpotifyService>()
                .ConfigureHttpClient(client =>
                {
                    client.BaseAddress = new Uri("https://api.spotify.com/v1/");
                });


            return services;
        }
    }
}
