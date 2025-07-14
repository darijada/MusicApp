using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MusicApp.Infrastructure.DependencyInjection;
using MusicApp.Infrastructure.Middleware;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// 1) Register infrastructure services (DbContext, Identity, JWT, email, refresh-token, settings)
builder.Services.AddInfrastructureServices(builder.Configuration);

// 2) Register application services (MediatR, AutoMapper)
var applicationAssembly = Assembly.Load("MusicApp.Application");
builder.Services.AddMediatR(applicationAssembly);
builder.Services.AddAutoMapper(applicationAssembly);

// 3) Register controllers and FluentValidation
builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(applicationAssembly));

// 4) Register Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "MusicApp API", Version = "v1" });
    // Define JWT Bearer auth for Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// 5) In Development: apply migrations and enable Swagger UI
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    //var db = scope.ServiceProvider.GetRequiredService<MusicApp.Infrastructure.Persistence.AppDbContext>();
    //db.Database.Migrate();

    // Enable middleware to serve generated Swagger as JSON endpoint
    app.UseSwagger();

    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.)
    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = string.Empty;               // Serve Swagger UI at application root
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MusicApp API V1");
    });
}

// 6) Global exception handling middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

// 7) Standard ASP.NET Core middleware
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// 8) Map controller endpoints
app.MapControllers();

// 9) Run the application
app.Run();
