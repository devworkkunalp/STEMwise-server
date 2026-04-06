using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace STEMwise.Infrastructure.Services;

/**
 * STEMwise Supabase JWT Configuration
 * Connects .NET Authentication to Supabase's JWT Signing Secret.
 */
public static class SupabaseAuthConfig
{
    public static void AddSupabaseAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSecret = configuration["Supabase:JwtSecret"];
        if (string.IsNullOrEmpty(jwtSecret))
        {
            // Fallback for local development if not in appsettings
            jwtSecret = "your-supabase-jwt-secret-here"; 
        }

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    // Supabase sends token in Authorization: Bearer <token>
                    return Task.CompletedTask;
                }
            };

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                ValidateIssuer = true,
                ValidIssuer = $"{configuration["Supabase:Url"]}/auth/v1",
                ValidateAudience = true,
                ValidAudience = "authenticated",
                ValidateLifetime = true
            };
        });
    }
}
