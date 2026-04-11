using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;



namespace STEMwise.Infrastructure.Services;

/**
 * STEMwise Supabase JWT Configuration
 * Connects .NET Authentication to Supabase's JWT Signing Secret.
 */
public static class SupabaseAuthConfig
{
    public static void AddSupabaseAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var supabaseUrl = configuration["Supabase:Url"];
        
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.Authority = $"{supabaseUrl}/auth/v1";
            options.RequireHttpsMetadata = true;
            
            var jwtSecret = configuration["Supabase:JwtSecret"];
            byte[] key;
            try
            {
                key = Convert.FromBase64String(jwtSecret);
                Console.WriteLine("[AUTH INFO] JWT Secret decoded as Base64.");
            }
            catch (Exception)
            {
                key = Encoding.UTF8.GetBytes(jwtSecret);
                Console.WriteLine("[AUTH INFO] JWT Secret fallback to UTF8 decoding.");
            }

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuers = new[] 
                { 
                    $"{supabaseUrl}/auth/v1",
                    supabaseUrl 
                },
                ValidateAudience = true,
                ValidAudience = "authenticated",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            options.Events = new JwtBearerEvents
            {
                OnChallenge = context =>
                {
                    Console.WriteLine($"[AUTH CHALLENGE] 401 Unauthorized for {context.Request.Path}");
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    Console.WriteLine("[AUTH SUCCESS] Token validated successfully.");
                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine($"[AUTH FAILURE] Path: {context.Request.Path}");
                    
                    if (context.Exception != null)
                    {
                        Console.WriteLine($"[AUTH ERROR] {context.Exception.Message}");
                        
                        if (context.Exception is SecurityTokenInvalidSignatureException)
                            Console.WriteLine("[AUTH DETAIL] Signature mismatch. Check if Supabase uses ES256 vs HS256.");
                        else if (context.Exception is SecurityTokenInvalidIssuerException)
                            Console.WriteLine("[AUTH DETAIL] Issuer mismatch. Expected vs Received.");
                        
                        if (context.Exception.InnerException != null)
                            Console.WriteLine($"[AUTH INNER ERROR] {context.Exception.InnerException.Message}");
                    }
                    else
                    {
                        Console.WriteLine("[AUTH ERROR] Unknown authentication failure (Exception was null).");
                    }
                        
                    return Task.CompletedTask;
                }
            };

        });
    }
}


