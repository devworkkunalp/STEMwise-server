using Microsoft.EntityFrameworkCore;
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

// Add Supabase Auth
builder.Services.AddSupabaseAuth(builder.Configuration);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
