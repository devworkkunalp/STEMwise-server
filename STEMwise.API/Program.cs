using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using STEMwise.Infrastructure.Data;
using STEMwise.Application.Interfaces;
using STEMwise.Infrastructure.ExternalAPIs;
using STEMwise.Infrastructure.Services;
using STEMwise.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add External API Clients
builder.Services.AddHttpClient<ICollegeScorecardClient, CollegeScorecardClient>();
builder.Services.AddHttpClient<IFrankfurterClient, FrankfurterClient>();
builder.Services.AddHttpClient<IBlsOewsClient, BlsOewsClient>();

// Add Domain Services
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IUniversityService, UniversityService>();
builder.Services.AddScoped<ISalaryService, SalaryService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<ICalculationService, CalculationService>();
builder.Services.AddScoped<IEnrichmentService, EnrichmentService>();
builder.Services.AddScoped<IScenarioService, ScenarioService>();

// Add Supabase Auth
builder.Services.AddSupabaseAuth(builder.Configuration);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

// Swagger/OpenAPI with Security
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "STEMwise API", Version = "v1" });

    // Add Security Definition for Bearer token
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter your Supabase JWT as: Bearer [token]"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference { Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});


var app = builder.Build();

// Global Traffic Logger
app.Use(async (context, next) =>
{
    Console.WriteLine($"[TRAFFIC] {context.Request.Method} {context.Request.Path}");
    await next();
});

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}


app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();





app.MapControllers();

app.Run();
