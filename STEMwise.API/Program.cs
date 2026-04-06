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
builder.Services.AddScoped<ICalculationService, CalculationService>();

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
