## MusicApp

A layered, extensible .NET 8 Web API for managing users and music playlists, with built-in Spotify integration and email-based account workflows.

### Key Features

- **Authentication & Authorization**  
  - Email confirmation, JWT access tokens & refresh tokens  
  - Login, logout, token refresh, account deactivation  
- **User Management**  
  - Register, confirm email, update profile, search users  
- **Playlist & Track Management**  
  - Create, read, update, delete playlists  
  - Add/remove tracks, share playlists via email  
- **Spotify Integration**  
  - Search and import tracks directly from Spotify  
- **Email Service**  
  - Transactional emails (confirmation, share notifications)  
- **Robust Architecture**  
  - Clean separation: Core → Application (CQRS & MediatR) → Infrastructure (EF Core & Identity) → API  
  - AutoMapper for DTO mappings, EF Core configurations for entities  

### Tech Stack

- **Platform:** .NET 8, ASP.NET Core Web API  
- **Patterns & Libraries:** MediatR (CQRS), AutoMapper, FluentValidation  
- **Persistence:** Entity Framework Core, ASP.NET Identity  
- **External Services:** Spotify API, SMTP/Email templates
- **Dev Tools:** SQL Server (or any EF Core provider), dotnet CLI  

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/)  
- SQL Server (or alternative)  
- Spotify Developer account (Client ID & Secret)  
- SMTP credentials or email-sending service (https://ethereal.email/create)

### Getting Started

1. **Clone & enter the repo**  
   ```bash
   git clone https://github.com/darijada/MusicApp
   cd MusicApp
   ```
2. **Configure**  
   - Set your connection string under `ConnectionStrings:DefaultConnection`  
   - Add JWT settings under `JwtSettings` 
   - Add Spotify credentials under `SpotifySettings`  
   - Configure SMTP under `SmtpSettings`
3. **Database migration**  
   ```bash
   cd MusicApp.Infrastructure
   dotnet ef migrations add Initial --project ../MusicApp.Infrastructure/MusicApp.Infrastructure.csproj --context AppDbContext
   dotnet ef database update --project ../MusicApp.Infrastructure/MusicApp.Infrastructure.csproj --context AppDbContext
   ```
4. **Run the API**  
   ```bash
   cd ../MusicApp.Api
   dotnet run
   ```
5. **Test Endpoints**  
   - Swagger UI at `https://localhost:5001/swagger`  

### Project Structure

```
/MusicApp.Core             → Domain entities & interfaces  
/MusicApp.Application      → Use cases (Commands/Queries) & DTOs  
/MusicApp.Infrastructure   → EF Core, Identity, external services  
/MusicApp.Api              → ASP.NET Core controllers & HTTP pipeline  
```
